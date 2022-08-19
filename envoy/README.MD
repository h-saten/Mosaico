# Envoy

Envoy is a proxy / API gateway for the microservices. It can work perfect on the local environment and simplify interaction with components. On the other hand, we might want to use Application Gateway when deployed to Azure.

## Routes

Current routing:

- [Identity] https://localhost:10000/id will propagate queries to https://localhost:4333/
- [Core] https://localhost:10000/core will propagate queries to https://localhost:5001/

If you want to see swagger definition, you can easily navigate to https://localhost:10000/id/swagger or  https://localhost:10000/core/swagger, but the execution of the API request does not work yet, so instead you can still use old  https://localhost:4333/id/swagger and https://localhost:5001/core/swagger.

## Certificates

If certificates expire, you can regenerate them by running following commands:
- `openssl req -config https.config -new -out csr.pem`
- `openssl x509 -req -days 365 -extfile https.config -extensions v3_req -in csr.pem -signkey key.pem -out https.crt`
  
If you want to see the localhost:10000 as trusted in google chrome, install https.crt on your machine in Trusted Root Authorities folder.

## Admin Panel

If you need to access admin panel of envoy, navigate to `http://localhost:9901/`

## Documentation

Read more about Envoy here https://www.envoyproxy.io/docs/envoy/latest/intro/what_is_envoy