# User

A NetBox user account.

> **Warning:** Managing users via IaC requires careful handling of passwords. Ensure passwords are stored securely and not committed to version control.

## API Endpoint

`/api/users/users/`

## Identifier

`username` - Username.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `username` | string | Yes | Username. |
| `password` | string | Yes | Password (write-only, never returned). |
| `firstName` | string | No | First name. |
| `lastName` | string | No | Last name. |
| `email` | string | No | Email address. |
| `isStaff` | string | No | Whether the user has staff access (true/false). |
| `isActive` | string | No | Whether the user account is active (true/false). |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource serviceAccount 'User' = {
  username: 'svc-automation'
  password: automationPassword
  firstName: 'Service'
  lastName: 'Account'
  email: 'svc-automation@example.com'
  isStaff: 'false'
  isActive: 'true'
}
```
