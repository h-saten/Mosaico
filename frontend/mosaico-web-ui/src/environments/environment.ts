export const environment = {
  production: false,
  gatewayUrl: "https://localhost:10000",
  relayUrl: 'http://localhost:5010',
  moralis: {
    "appId": "B9T7DU7nUl8MB0pbpidma7fGxBxyIzn2OQfyPHMY",
    "serverUrl": "https://qs3yg4e96r9m.usemoralis.com:2053/server"
  },
  appVersion: "0.0.1",
  transak: {
    "apiKey": "947f1c90-d070-48ef-8dd0-bf7d1f841a40",
    "environment": "PRODUCTION",
    "themeColor": "#0063F5",
    "fiatCurrency": "PLN",
    "hostURL": "http://localhost:4200/",
    "widgetHeight": "550px",
    "widgetWidth": "450px",
    "countryCode": "PL",
    "hideMenu": true,
    "isFeeCalculationHidden": false,
    "isDisableCrypto": true,
    "hideExchangeScreen": true,
    "disableWalletAddressForm": true
  },
  ckEditorLicenseKey: "KjB2Hjtp2UMyy6B8YJeA9qhAQmeycUE+Hfi7OB0Dc1J8ziR0qzU=",
  auth: {
    issuer: 'https://localhost:49153',
    clientId: 'spa-tokenizer',
    redirectUri: 'http://localhost:4200/signin',
    responseType: "code",
    scope: "openid profile tokenizerapi.full IdentityServerApi offline_access",
    postLogoutRedirectUri: 'http://localhost:4200/',
    secureRoutes: ['https://localhost:10000']
  },
  beamerProductKey: 'PQJHwlMF41668',
  rampApiKey: '2up7zmsx65jpx6q3jbb6jh72b5pcdsdof4tgs65t',
  signalrConnectionType: 'longPolling',
  kycProvider: 'PASSBASE',
  basisIdKey: "prod-ljUatEbtVzqYUgQoadXbmmBGahKjBQeW",
  passbaseKey: "SIf7ruuVm4bgPdzVfyC80rVa8sETjFqnN2u3h6vSUFTPGsLj9AooIxrIDhno9BmI",
  walletConnectId: "f77a8527520189f4936d2d2afe85f1b2"
};
