# UserGroup

A user group for permissions.

## API Endpoint

`/api/users/groups/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `description` | string | No | A brief description. |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource networkAdmins 'UserGroup' = {
  name: 'Network Administrators'
  description: 'Full access to DCIM and IPAM resources'
}
```
