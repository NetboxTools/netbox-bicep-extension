targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string
@secure()
param defaultPassword string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Rack Roles ────────────────────────────────────────

resource roleCompute 'RackRole' = {
  name: 'Compute'
  slug: 'compute'
  color: '4caf50'
  description: 'Compute/server racks'
}

resource roleNetworking 'RackRole' = {
  name: 'Networking'
  slug: 'networking'
  color: '2196f3'
  description: 'Network equipment racks'
}

// ─── Locations ─────────────────────────────────────────

resource locBuilding1 'Location' = {
  name: 'Building 1'
  slug: 'building-1'
  site: 1  // Update with actual Site ID
  status: 'active'
  description: 'Main building'
}

resource locFloor2 'Location' = {
  name: 'Floor 2'
  slug: 'floor-2'
  site: 1  // Update with actual Site ID
  status: 'active'
  description: 'Second floor server room'
}

// ─── Racks ─────────────────────────────────────────────

resource rackA01 'Rack' = {
  name: 'A-01'
  site: 1  // Update with actual Site ID
  status: 'active'
  uHeight: '42'
  description: 'Primary compute rack'
}

resource rackB01 'Rack' = {
  name: 'B-01'
  site: 1  // Update with actual Site ID
  status: 'active'
  uHeight: '42'
  description: 'Network rack'
}

// ─── User Groups ───────────────────────────────────────

resource groupNetworkAdmins 'UserGroup' = {
  name: 'Network Admins'
  description: 'Network operations team'
}

resource groupReadOnly 'UserGroup' = {
  name: 'Read Only'
  description: 'Read-only access for auditors'
}

// ─── Users ─────────────────────────────────────────────

resource userJane 'User' = {
  username: 'jane.doe'
  password: defaultPassword
  firstName: 'Jane'
  lastName: 'Doe'
  email: 'jane.doe@contoso.com'
  isActive: 'true'
  isStaff: 'false'
}

resource userBob 'User' = {
  username: 'bob.smith'
  password: defaultPassword
  firstName: 'Bob'
  lastName: 'Smith'
  email: 'bob.smith@contoso.com'
  isActive: 'true'
  isStaff: 'true'
}
