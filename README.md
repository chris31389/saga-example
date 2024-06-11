# saga-example

## Dependencies

### RabbitMq

[Docker tutorial for RabbitMq](https://www.svix.com/resources/guides/rabbitmq-docker-setup-guide/#step-1-pulling-the-rabbitmq-docker-image)

```cmd
docker pull rabbitmq:3-management
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
docker start rabbitmq
```


### PostreSQL

[Docker tutorial for PostgreSQL](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)
https://www.dbvis.com/thetable/how-to-set-up-postgres-using-docker/

```cmd
docker pull postgres
docker run --name postgres_container -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 -v postgres_data:/var/lib/postgresql/data postgres
docker start postgres_container

docker run --name saga-postgres -e POSTGRES_PASSWORD=mysecretpassword -d postgres
docker start saga-postgres
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


