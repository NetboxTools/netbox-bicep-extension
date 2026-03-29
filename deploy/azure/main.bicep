targetScope = 'resourceGroup'

// ──────────────────────────────────────────────
// Parameters
// ──────────────────────────────────────────────

@description('Azure region for all resources')
param location string = 'swedencentral'

@secure()
@description('Password for the PostgreSQL admin user (min 8 chars, must include upper, lower, number)')
param postgresPassword string

@secure()
@description('Django SECRET_KEY — generate with: python3 -c "import secrets; print(secrets.token_urlsafe(50))"')
param netboxSecretKey string

@description('NetBox superuser username')
param superuserName string = 'admin'

@description('NetBox superuser email')
param superuserEmail string = 'admin@example.com'

@secure()
@description('NetBox superuser password')
param superuserPassword string


// ──────────────────────────────────────────────
// Variables
// ──────────────────────────────────────────────

var suffix = uniqueString(resourceGroup().id)
var netboxFqdn = 'ca-netbox.${cae.properties.defaultDomain}'

// ──────────────────────────────────────────────
// Log Analytics Workspace (required by Container Apps)
// ──────────────────────────────────────────────

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'log-netbox'
  location: location
  properties: {
    sku: { name: 'PerGB2018' }
    retentionInDays: 30
  }
}

// ──────────────────────────────────────────────
// Container Apps Environment
// ──────────────────────────────────────────────

resource cae 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: 'cae-netbox'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
  }
}

// ──────────────────────────────────────────────
// Azure Database for PostgreSQL — Flexible Server
// ──────────────────────────────────────────────

resource postgres 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
  name: 'psql-netbox-${suffix}'
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    version: '16'
    administratorLogin: 'netboxadmin'
    administratorLoginPassword: postgresPassword
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource postgresDatabase 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2022-12-01' = {
  parent: postgres
  name: 'netbox'
}

// Allow connections from Azure services (Container Apps)
resource postgresFirewall 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2022-12-01' = {
  parent: postgres
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// The NetBox Docker image defaults to sslmode=prefer. Disabling the server-side
// requirement avoids extra Django configuration. For production, set DB_SSLMODE=require
// in the container env and remove this override.
resource postgresSslConfig 'Microsoft.DBforPostgreSQL/flexibleServers/configurations@2022-12-01' = {
  parent: postgres
  name: 'require_secure_transport'
  properties: {
    value: 'off'
    source: 'user-override'
  }
}

// ──────────────────────────────────────────────
// Azure Cache for Redis
// ──────────────────────────────────────────────

resource redis 'Microsoft.Cache/redis@2023-08-01' = {
  name: 'redis-netbox-${suffix}'
  location: location
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
    enableNonSslPort: false
    minimumTlsVersion: '1.2'
    redisConfiguration: {}
  }
}

// ──────────────────────────────────────────────
// Container App — NetBox
// ──────────────────────────────────────────────

resource netbox 'Microsoft.App/containerApps@2023-05-01' = {
  name: 'ca-netbox'
  location: location
  properties: {
    managedEnvironmentId: cae.id
    configuration: {
      secrets: [
        { name: 'db-password',        value: postgresPassword }
        { name: 'redis-password',     value: redis.listKeys().primaryKey }
        { name: 'secret-key',         value: netboxSecretKey }
        { name: 'superuser-password', value: superuserPassword }
      ]
      ingress: {
        external: true
        targetPort: 8080
        transport: 'http'
      }
    }
    template: {
      containers: [
        {
          name: 'netbox'
          image: 'netboxcommunity/netbox:latest'
          resources: {
            cpu: json('1.0')
            memory: '2Gi'
          }
          env: [
            // ── PostgreSQL ──
            { name: 'DB_HOST',       value: postgres.properties.fullyQualifiedDomainName }
            { name: 'DB_PORT',       value: '5432' }
            { name: 'DB_NAME',       value: 'netbox' }
            { name: 'DB_USER',       value: 'netboxadmin' }
            { name: 'DB_PASSWORD',   secretRef: 'db-password' }
            { name: 'DB_SSLMODE',    value: 'require' }
            { name: 'DB_CONN_MAX_AGE', value: '300' }

            // ── Redis — tasks (database 0) ──
            { name: 'REDIS_HOST',       value: redis.properties.hostName }
            { name: 'REDIS_PORT',       value: '6380' }
            { name: 'REDIS_PASSWORD',   secretRef: 'redis-password' }
            { name: 'REDIS_DATABASE',   value: '0' }
            { name: 'REDIS_SSL',        value: 'true' }

            // ── Redis — cache (database 1, same instance) ──
            { name: 'REDIS_CACHE_HOST',       value: redis.properties.hostName }
            { name: 'REDIS_CACHE_PORT',       value: '6380' }
            { name: 'REDIS_CACHE_PASSWORD',   secretRef: 'redis-password' }
            { name: 'REDIS_CACHE_DATABASE',   value: '1' }
            { name: 'REDIS_CACHE_SSL',        value: 'true' }

            // ── NetBox application ──
            { name: 'SECRET_KEY',           secretRef: 'secret-key' }
            { name: 'API_TOKEN_PEPPER_1',   secretRef: 'secret-key' }
            { name: 'ALLOWED_HOSTS',        value: '*' }
            { name: 'CSRF_TRUSTED_ORIGINS', value: 'https://${netboxFqdn}' }

            // ── Security ──
            { name: 'LOGIN_REQUIRED',              value: 'true' }
            { name: 'LOGIN_TIMEOUT',               value: '86400' }
            { name: 'CORS_ORIGIN_ALLOW_ALL',       value: 'false' }
            { name: 'CENSUS_REPORTING_ENABLED',     value: 'false' }

            // ── Housekeeping ──
            { name: 'CHANGELOG_RETENTION',  value: '365' }
            { name: 'JOB_RETENTION',        value: '90' }
            { name: 'TIME_ZONE',            value: 'Europe/Stockholm' }
            { name: 'MAX_PAGE_SIZE',        value: '1000' }
            { name: 'GRAPHQL_ENABLED',      value: 'true' }
            { name: 'METRICS_ENABLED',      value: 'false' }

            // ── Superuser (created on first startup) ──
            { name: 'SKIP_SUPERUSER',       value: 'false' }
            { name: 'SUPERUSER_NAME',       value: superuserName }
            { name: 'SUPERUSER_EMAIL',      value: superuserEmail }
            { name: 'SUPERUSER_PASSWORD',   secretRef: 'superuser-password' }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 1
      }
    }
  }
  dependsOn: [
    postgresDatabase
    postgresFirewall
    postgresSslConfig
  ]
}

// ──────────────────────────────────────────────
// Outputs
// ──────────────────────────────────────────────

output netboxUrl string      = 'https://${netbox.properties.configuration.ingress.fqdn}'
output postgresHost string   = postgres.properties.fullyQualifiedDomainName
output redisHost string      = redis.properties.hostName
