targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Tenant Groups ────────────────────────────────

resource tgInternal 'TenantGroup' = {
  name: 'Internal'
  description: 'Internal departments'
}

resource tgCustomers 'TenantGroup' = {
  name: 'Customers'
  description: 'External customers'
}

// ─── Tenants (referencing groups) ─────────────────

resource tenantIT 'Tenant' = {
  name: 'IT Operations'
  group: tgInternal.id
  description: 'IT Operations department'
}

resource tenantAcme 'Tenant' = {
  name: 'Acme Corp'
  group: tgCustomers.id
  description: 'Acme Corporation — key customer'
}

// ─── Contact Groups ───────────────────────────────

resource cgOperations 'ContactGroup' = {
  name: 'Operations Team'
  description: 'Infrastructure operations contacts'
}

resource cgManagement 'ContactGroup' = {
  name: 'Management'
  description: 'Management contacts'
}

// ─── Contact Roles ────────────────────────────────

resource crPrimary 'ContactRole' = {
  name: 'Primary Contact'
  description: 'Primary point of contact'
}

resource crEscalation 'ContactRole' = {
  name: 'Escalation Contact'
  description: 'Escalation point for critical issues'
}

// ─── Contacts ─────────────────────────────────────

resource contactAnna 'Contact' = {
  name: 'Anna Lindqvist'
  title: 'Network Engineer'
  phone: '+46-8-555-0100'
  email: 'anna.lindqvist@contoso.com'
  description: 'Primary network engineer — Stockholm'
}

resource contactErik 'Contact' = {
  name: 'Erik Johansson'
  title: 'Infrastructure Manager'
  phone: '+46-8-555-0200'
  email: 'erik.johansson@contoso.com'
  description: 'Infrastructure manager — all sites'
}

// ─── Platforms ─────────────────────────────────────

resource platIOS 'Platform' = {
  name: 'Cisco IOS-XE'
  description: 'Cisco IOS-XE operating system'
}

resource platJunOS 'Platform' = {
  name: 'Juniper Junos'
  description: 'Juniper Junos operating system'
}

resource platESXi 'Platform' = {
  name: 'VMware ESXi 8'
  description: 'VMware ESXi hypervisor v8'
}
