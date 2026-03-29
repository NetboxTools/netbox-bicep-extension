# IKEProposal

An IKE (Internet Key Exchange) proposal.

## API Endpoint

`/api/vpn/ike-proposals/`

## Identifier

`name` - Unique name.

## Properties

| Property | Type | Required | Description |
|---|---|---|---|
| `name` | string | Yes | Unique name. |
| `authenticationMethod` | string | Yes | Authentication method (e.g. preshared-keys, certificates, rsa-signatures). |
| `encryptionAlgorithm` | string | Yes | Encryption algorithm (e.g. aes-128-cbc, aes-256-cbc, 3des-cbc). |
| `authenticationAlgorithm` | string | No | Authentication/hash algorithm (e.g. hmac-sha1, hmac-sha256, hmac-md5). |
| `group` | string | Yes | Diffie-Hellman group (e.g. 1, 2, 5, 14, 19, 20). |
| `saLifetime` | string | No | SA lifetime in seconds. |
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
resource ikeProposal 'IKEProposal' = {
  name: 'IKE-AES256-SHA256-DH14'
  authenticationMethod: 'preshared-keys'
  encryptionAlgorithm: 'aes-256-cbc'
  authenticationAlgorithm: 'hmac-sha256'
  group: '14'
  saLifetime: '86400'
  description: 'Standard IKE proposal with AES-256'
}
```
