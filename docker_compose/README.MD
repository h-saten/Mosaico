### Build docker file

`docker build . --file Dockerfile.mac.SQLDatabase -t macsql`

### Run docker image

`docker run --rm -e "SA_PASSWORD=Mosaico2021!" -e "MSSQL_TCP_PORT=1433" -e "ACCEPT_EULA=Y" -p 1433:1433 macsql`

