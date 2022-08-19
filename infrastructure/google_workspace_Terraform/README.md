# Learn Terraform Google Workspace Provider

https://learn.hashicorp.com/tutorials/terraform/google-workspace?in=terraform/it-saas

PROVIDE terraform.tfvars file with domain name

first_name,last_name,email,password,password_hash_function,recovery_email,dept,title
Michal,Czaja,m.czajadev@mosaico.dev,zaq1@WSX,MD5,michalczaja2292@gmail.com,development,admin

https://support.google.com/a/answer/6357481
FOR SLACK SAML SET UP

SSO_URL : https://accounts.google.com/o/saml2/idp?idpid=C01sp2ud6
ENTITY ID : https://accounts.google.com/o/saml2?idpid=C01sp2ud6

CERTIFICATE :
-----BEGIN CERTIFICATE-----
MIIDdDCCAlygAwIBAgIGAX+SCqX7MA0GCSqGSIb3DQEBCwUAMHsxFDASBgNVBAoTC0dvb2dsZSBJ
bmMuMRYwFAYDVQQHEw1Nb3VudGFpbiBWaWV3MQ8wDQYDVQQDEwZHb29nbGUxGDAWBgNVBAsTD0dv
b2dsZSBGb3IgV29yazELMAkGA1UEBhMCVVMxEzARBgNVBAgTCkNhbGlmb3JuaWEwHhcNMjIwMzE2
MDkyNDA1WhcNMjcwMzE1MDkyNDA1WjB7MRQwEgYDVQQKEwtHb29nbGUgSW5jLjEWMBQGA1UEBxMN
TW91bnRhaW4gVmlldzEPMA0GA1UEAxMGR29vZ2xlMRgwFgYDVQQLEw9Hb29nbGUgRm9yIFdvcmsx
CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpDYWxpZm9ybmlhMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8A
MIIBCgKCAQEAlpSvaVS1sT66BcfEIeVUFTl3ZLP22Rn2m5JP4Uq+o0WdppfmO+2YOs7nAehQn3lv
tc85OuGfoVLyG35FIWhOXkjlxU3ypi8W6zJ6SmaITTThtywQyLd38PJDKN35c6tPTeoadHgl41rS
COo/sqSETmqtwU/K+KFg38TwWnTmWQ9/MzaFiMXJxj0TUohe8AUyqH1PSwFlGasyz8zQLlaQ2F3u
SFmxrLalhw7SW7S1SD7AzZaex22F7LK2BKIuK1gWWWVXMPQvD03G24P7db5zm1Q3icqZnDEQovm0
R7dtIsFrElGSRXWyNh/AQIqynbRJyM2s5C+LEiJXQUStGB4hbwIDAQABMA0GCSqGSIb3DQEBCwUA
A4IBAQAZJC6un+YRbzYutHSjk1x/WNFz+3+eWl++1I0KtovNKCDZ5F3C+5edtTTxRJvhy/rl8geM
1Zgq1NTOHsdg2m4P4J8vnNUif3BCsk5dDH6rSH+ug7h9njil2cbfvPAmd9HaQ/9r4L5/wpomawm2
3zXgnCpIMZ9Ch5/P7sSCf4FBboWTwUMYhjU9XsmGx58Em8hgq24qduDFPoh3LVA5ZB6gZH+ItVWZ
GvbXzPJVn9FPNLvG863CfABZ3RA2MqN7W23QStnOxzia3vn9tS7F+x0+JFMI+/stX4d4rH64yJb5
PYudPRKA8I3R/sQW5MVFA3xCdBkau8c0NRoMlZDGluZA
-----END CERTIFICATE-----

SHA256 FINGERPRINT: 51:A3:17:84:64:27:8C:EC:C5:99:3B:8B:32:1F:B9:09:18:87:CB:9A:BB:8F:E5:1B:1A:00:B3:1D:E4:80:61:56