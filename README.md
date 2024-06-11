# saga-example

## Dependencies

### Shared Network

```
docker network create saga_net
```

### RabbitMq

[Docker tutorial for RabbitMq](https://www.svix.com/resources/guides/rabbitmq-docker-setup-guide/#step-1-pulling-the-rabbitmq-docker-image)

```cmd
docker pull rabbitmq:3-management
docker run -d --network saga_net --name rabbitmq --hostname rabbitmqhost -p 5672:5672 -p 15672:15672 rabbitmq:3-management
docker start rabbitmq
```


### PostreSQL

[Docker tutorial for PostgreSQL](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)
https://www.dbvis.com/thetable/how-to-set-up-postgres-using-docker/

```cmd
docker pull postgres
docker run --name postgres_container -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 -v postgres_data:/var/lib/postgresql/data postgres
docker start postgres_container
```

## Our App

### Building

#### Invoices.Api

```
docker build -t invoices-api -f Invoices.Api.Dockerfile .
docker run -d --network saga_net -e ConnectionStrings:RabbitMq="amqp://rabbitmqhost:5672" -p 8080:8080 invoices-api 
```

`POST http://localhost:8080/invoice`

#### Invoices.Worker

```
docker build -t invoices-worker-image -f Invoices.Worker.Dockerfile .
docker create --name invoices-worker invoices-worker-image
docker run -p 8080:8080 invoices-worker -e ConnectionStrings:RabbitMq="amqp://localhost:5672"
```

#### Debtors.Worker

```
docker build -t debtors-worker-image -f Debtors.Worker.Dockerfile .
docker create --name debtors-worker debtors-worker-image
docker run -p 8080:8080 debtors-worker
```

#### Emails.Worker

```
docker build -t email-worker-image -f Email.Worker.Dockerfile .
docker create --name email-worker email-worker-image
docker run -p 8080:8080 email-worker
```

