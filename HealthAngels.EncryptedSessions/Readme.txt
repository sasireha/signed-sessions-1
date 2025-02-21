Description
This package will encrypt the session data using AES256 algorithm and store it in a distributed cache.

Encryption
AES256 algorithm is used for encryption and once encrypting the data using nonce (which is 12 bytes) it store's the data in the form of Json string as below.

The Json string contains the following properties.
1) cypherData
2) nonce
3) tag

cypherData is AES256 encrypted and then base64 encoded into a string. The nonce and tag are only base64 encoded. You need a 256 bit (=32 byte) AES key to decrypt the values.

"{\"cypherData\":\"M/gMy/awKkTc4s+15tD3J/FGRrVVP80nQsGMihszNGGBkmK6mbuWTmaRcV2CVQ7k5CNA9WwpKzJoyNFvX+fOnP2zrkhDhJH1ZmTCIlPA9VZWeIp2whtILykaocv3yjdrtw0nzdTeZYOAAUX/TfLTADXwjRyBpspQhsrjLsktVSal2csdmQ9uQHBxB6PAc+4fXqkvnc4ooV0VmaeUTz8ASQJ/51Od2PCF4swf+OiYF7xX7tsuoSzbWuLuQLc5lm0yp9nh/cWny0BYHxmYt/wTS50V56b1OKVeIg0z2/SyOPrEv/Ox/IWRG23gMjs2s7tpO4IDjM1N6vPGXq1jr8OGyoidHhAwlUnyC271Mlw5BT0BYuI8xBeH92rip6hH5sEWCx7hDQwFLI1YVuM8YFb6xwdnL/41JM3tu6q+HSZ7ZmZFCUWlBGOEigFXU9/gD7Ks1rH06saqLKhsfZvDedmCx1LYUVryfjVr7SlkiLuLQKPA+yuo9IUEF6LLfTS8oB6yCzCc8bJATEhRqvLwvelUkB+vdhaqFy2RGaCRkJjR86l9X8xdfYLEuwRliXuT3laZk/850qJ7nh2EilLmEh9yzGMQFhRsFlwCyBvwhoVAHUsj3/RUzx6mGh5/dOpFM0D2iJFCzerUsr54OgkxOtZJ3fDATo6nwGglx7MhwzarsGyrj3m6Vq68I+1cxa7BQr1pAVHv2yZI204zOA05DysjquCFEpDoBV4RuQ1SgYZhBUD46OjsKICj3U4yA22m5rkwq1Y49kioxxv8BtQYc+Il3hnPsvfbF0bnWh1uFsPL+5POC1PbeqTP8oeyUAhybuVTVcb520QwxR4Z3euPJ/f5MK5e+jsnY4O8E6yw4oVdW2PilV8GPJzKDcWkVJkFnviTisgDZ7xY6Lmh0sBSqXGtwxcTgVDiqSFpu2s+kdKOkhlp2ZBONSIFdUp0gQB9/FMSFYG7YWnOZzEcEqUVyV1+bO1unoJctt93C/hS6asS4upQ06jcZGjBnB2Smy04HTvZ0zTnv8VB5tTA7m9nx0+ytQliQMMUzJ7UGlPG9eNEOZEMrE8qa8ReONsC+AWQ11mB+OLukQG0m2h9Hmh8r7GE9mrF0NTDh4tVvRxWklN8xOEMnSL2QXgGsThtDigqSrT9PlhYpqAaxdtXy+w4WROh8LSua4gLqMXdT9si96xQrHDg2kanyHQWpif5Us6MC9FI5zLDtCMpetcZnp3HH21XUNU0FEVVFRDswutGKwSw++1UAZqIu2c1sKl5pAf8q93IPGBeY8fObSWsOQ8qJCN7CXPVySopfBmxWhM2OttQ/MuqXtp1SlpBfziIdk73zUXjhAk7pz9eRw3QxNpZttWDI9rrUj/B5UZi7O4CGU1/7wDICBpx/PwC1DkAA8Yujl+rR6Y4QiIXsillshJfxPP5S7AW4e88o0vNjXEmTQG4NTXzk5Sb7wLfAqFimML3xTspSrWlmwEeRoXf4jwpXmnb/JG0YKZSUYRcEYEEbQn4WsjGlFrj7k78pu3SqeoozCAXo74gcJE2xafYAyZ95jjyD4D6y3y4+KN6GSwRJCkjqCeJTkRO5EgWovctimk6eM+Ptsv9SEcH39/Cifge2M6PxTOFn7pohl2n7fXJ8A3F1lKbcOkQJJOydlA2ClAbkYRxm/3Kqlp49FespIwt0cDLAbUf/jFLRnps1sa/fc1oWnCl3bVxEjptiXDp4PcUkAG4OIy2UM8zhSSPa3PKaEjuc2dJZH61ym/HQcEIshjvNLpT6K0IQIL2tDlhagjua6pJ+3s2EERsz8x8D8P28EKJgbasiv0yrdo0EeIiMvZohvDUGVDejeuQgSX+6nnZ57gUKM0jE6mO+b1TaiRhJ2K5BVi+ckLChU0svAANx0NSNAbnUV/GTkM5fihtRIE9qwg+qFLvU3qpyIGw8FFAOOSzsSIT8Mnb191hDtnJ6xzqSLJOu8WJPVEjs4W8z02MUVgYoIIBb/5RviF7rGzfkCQwAuZaOBVEV0yC2aeGVi5qlz2QE010D/+YkQjIXJzy4z+2kEwF8OFpucwuZW/aHtMBVOH29NPSQ2VyOt1g+8Ej2o079UnGz0OZ4blavYBUFjKGlpEHBUfeL+rdPOBR1SsslA5NE7xmfuyOw/cHUleFTdLOxrqqFR1sQfJ2Tfy4s+xg76Fh9Sopvc+rPB/YMlcoGg3m6CCr/wlOy3I3Lq4Vo+XFBLsc3CWlRt/hmSYwjKvVdvbzDQheSqhagRNJekujZj2TWA1EvO7RPrum4d6XJ808rEZXWRqO6HQdclLXLhQj6cNDfuffJOJAmZzTIx8Em7u+T8MTB5aNia9wZYULSDso2Byq7hPFFPhLdSCE6PTrnDS+5wW78vAOMoD/Md/2fOhIb/wPWinaJPQ2+vFpnspyiuHai5SJabwGeePiGSY77cj04/KE3C7woy+4LlYlm14AN8Iuqi8w7uvQckF78tqLOCSlpDiLgFRCu7Femd7EATdjSheUCSxcTMTvHASHXdK/qd+rSx5I6IrU3ouYLZ0C4LO8uenc+VJdxD3wGjiy+6Cq3fz76pDndFihcJMm0xQk6ZFr5F2Ao1s+o0tdLPphMHJobmNNLudhMtAiXKz9JSoZG2guWfzWlMBZrT3cqkpcNhYZ/lOueTwK/l5/qBA6pnfgW4crD44Jz50LorDRFyRk/tZ5cwNPqkCRf5vpO9mig90kzjLwmHbKNhq1I3sayd+yGTlBWOl7fevaMm9v691gG+c2TWju/5HLagLBjzWYPCO4vhzPKdRc0Y1wBValI6ThRsJ6HKh8XmsyHo4VXmZodiQB04zYb6ERDLVqXtqofZKTdf+OsVmr19/OZrKpd48NhzsrExHz2W1vM04K9OlgjdC8upPsaUE3GLmlr+vxR2IXF1m/XNG31RyFQVb2thqdb71b3ihFFy5uW69K3UCOhg3RHMLXmdWbI9FtNWyTnuvtEKnM6Cw=\",\"nonce\":\"7ucpOyxZwPqCAWZb\",\"tag\":\"zenNSMNPtTo73/npD71q5A==\"}"

Installation

Add the package to the Project

```
  dotnet add package HealthAngels.EncryptedSessions --version 1.0.11
```

Add the configuration dependencies in the startup

AesCryptoConfig contains the key used for encryting the data.

```
services.Configure<AesCryptoConfig>(Configuration.GetSection(nameof(AesCryptoConfig)));
```

```
Config.yaml

AesCryptoConfig:
  AesEncryptionKey: "256 bit (=32 byte) AES key"
```