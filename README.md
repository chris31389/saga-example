# saga-example

## Dependencies

### PostreSQL

[Docker tutorial for PostgreSQL](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)

`docker pull postgres`
`docker run --name saga-postgres -e POSTGRES_PASSWORD=mysecretpassword -d postgres`

## Our App

### Building

```
cd .\InvoiceApi\
docker build -t invoice-api-image -f Dockerfile .
docker create --name invoice-api invoice-api-image
docker run -p 8080:8080 invoice-api
```

`POST http://localhost:8080/invoice`


