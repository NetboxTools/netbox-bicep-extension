# VLANTranslationPolicy

A VLAN translation policy.

## API Endpoint

`/api/ipam/vlan-translation-policies/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
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
resource translationPolicy 'VLANTranslationPolicy' = {
  name: 'Edge Translation Policy'
  description: 'VLAN translation for edge sites'
}
```
