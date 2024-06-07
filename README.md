# saga-example

## Dependencies

### RabbitMq

[Docker tutorial for RabbitMq](https://www.svix.com/resources/guides/rabbitmq-docker-setup-guide/#step-1-pulling-the-rabbitmq-docker-image)

```cmd
docker pull rabbitmq:3-management
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```


### PostreSQL

[Docker tutorial for PostgreSQL](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)

```cmd
docker pull postgres
docker run --name saga-postgres -e POSTGRES_PASSWORD=mysecretpassword -d postgres
```

## Our App

### Building

```
cd .\InvoiceApi\
docker build -t invoice-api-image -f Dockerfile .
docker create --name invoice-api invoice-api-image
docker run -p 8080:8080 invoice-api
```

`POST http://localhost:8080/invoice`


