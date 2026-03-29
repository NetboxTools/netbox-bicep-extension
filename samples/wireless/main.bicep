targetScope = 'local'

@secure()
param netboxToken string
param netboxUrl string

extension netbox with {
  url: netboxUrl
  token: netboxToken
}

// ─── Wireless LAN Groups ──────────────────────────

resource wlgCorporate 'WirelessLANGroup' = {
  name: 'Corporate WiFi'
  description: 'Corporate wireless networks'
}

resource wlgGuest 'WirelessLANGroup' = {
  name: 'Guest WiFi'
  description: 'Guest wireless networks'
}

// ─── Wireless LANs (referencing groups) ───────────

resource wlanCorpMain 'WirelessLAN' = {
  ssid: 'CONTOSO-CORP'
  status: 'active'
  group: wlgCorporate.id
  authType: 'wpa-enterprise'
  authCipher: 'aes'
  description: 'Corporate 802.1X WiFi'
}

resource wlanCorpIoT 'WirelessLAN' = {
  ssid: 'CONTOSO-IOT'
  status: 'active'
  group: wlgCorporate.id
  authType: 'wpa-personal'
  authCipher: 'aes'
  authPsk: 'IoTDevices2024!'
  description: 'IoT device network'
}

resource wlanGuest 'WirelessLAN' = {
  ssid: 'CONTOSO-GUEST'
  status: 'active'
  group: wlgGuest.id
  authType: 'open'
  description: 'Guest captive portal WiFi'
}

// ─── Tags ─────────────────────────────────────────

resource tagWifi 'Tag' = {
  name: 'wifi'
  color: '9c27b0'
  description: 'Wireless infrastructure'
}

resource tagProduction 'Tag' = {
  name: 'production'
  color: '4caf50'
  description: 'Production environment'
}
