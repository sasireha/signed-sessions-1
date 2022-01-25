##Description
This package will encrypt the session data using AES256 algorithm and store it in a distributed cache.

###Encryption
AES256 algorithm is used for encryption and once encrypting the data using nonce (which is 12 bytes) it store's the data in the form of Json string as below.

The Json string contains the following properties.
1) cypherData
2) nonce
3) tag

cypherData is AES256 encrypted and then base64 encoded into a string. The nonce and tag are only base64 encoded. You need a 256 bit (=32 byte) AES key to decrypt the values.

```
{\"CypherData\":\"1Zfszq8CnWEqrg==\",\"Nonce\":\"V4wB4KBhNPz1pU50\",\"Tag\":\"JbJhOGJhD798KM3q8MrCeA==\"}
```

###Installation

1) Add the package to the Project

`dotnet add package HealthAngels.EncryptedSessions --version 1.0.11`

2) Add the configuration dependencies in the startup

AesCryptoConfig contains the key used for encryting the data.

```
services.Configure<AesCryptoConfig>(Configuration.GetSection(nameof(AesCryptoConfig)));
```

```
Config.yaml

AesCryptoConfig:
  AesEncryptionKey: "256 bit (=32 byte) AES key"
```

