# Contact

A contact person.

## API Endpoint

`/api/tenancy/contacts/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `title` | string | No | Job title. |
| `phone` | string | No | Phone number. |
| `email` | string | No | Email address. |
| `address` | string | No | Mailing address. |
| `link` | string | No | URL link. |
| `description` | string | No | A brief description. |
| `comments` | string | No | Free-text comments (markdown supported). |

## Output Properties

| Property | Type | Description |
|---|---|---|
| `id` | string | NetBox internal ID (read-only, returned after create/update). |
| `url` | string | API URL for this resource. |
| `display` | string | Display name. |

## Bicep Example

```bicep
resource nocLead 'Contact' = {
  name: 'Jane Smith'
  title: 'NOC Team Lead'
  phone: '+46-8-555-1234'
  email: 'jane.smith@example.com'
  description: 'Primary NOC contact for Stockholm DC'
}
```
