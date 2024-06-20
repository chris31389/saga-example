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

### MongoDb

https://www.geeksforgeeks.org/how-to-run-mongodb-as-a-docker-container/

```cmd
docker pull mongo:latest
docker run -d --network saga_net -p 27017:27017 --name=mongo --hostname mongohost -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=password -e MONGO_INITDB_DATABASE=orders-db mongo:latest
```

### PostreSQL

[Docker tutorial for PostgreSQL](https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/)
https://www.dbvis.com/thetable/how-to-set-up-postgres-using-docker/

```cmd
docker pull postgres
docker run -d --network saga_net --name postgres --hostname postgreshost -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -v postgres_data:/var/lib/postgresql/data postgres
docker start postgres
```

### Cosmos 

https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=docker-linux%2Ccsharp&pivots=api-nosql

```
docker pull mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest
docker run -d --network saga_net --name cosmos --hostname cosmoshost -p 8081:8081 -p 10250-10255:10250-10255 --interactive --tty mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest
```

https://localhost:8081/_explorer/index.html

#### Install the certificate

https://stackoverflow.com/a/77825243/2069306

## Our App

### Building

#### Invoices.Api

```
docker build -t invoices-api -f Invoices.Api.Dockerfile .
docker rm invoices-api
docker run -d --network saga_net --name invoices-api -p 8080:8080 -e ConnectionStrings:RabbitMq="amqp://rabbitmqhost:5672" -e ConnectionStrings:Mongo="mongodb://admin:password@mongohost:27017/orders-db?authSource=admin" invoices-api 
```

`POST http://localhost:8080/invoice`

#### Invoices.Worker

https://www.learnentityframeworkcore.com/migrations#:~:text=Migrations%20are%20enabled%20by%20default,commands%20to%20create%20a%20migration.

```
docker build -t invoices-worker -f Invoices.Worker.Dockerfile .
docker rm invoices-worker
docker run -d --network saga_net --name invoices-worker -e ConnectionStrings:RabbitMq="amqp://rabbitmqhost:5672" -e ConnectionStrings:Mongo="mongodb://admin:password@mongohost:27017/orders-db?authSource=admin" invoices-worker 

```

#### Debtors.Worker

```
docker build -t debtors-worker -f Debtors.Worker.Dockerfile .
docker rm debtors-worker
docker run -d --network saga_net --name debtors-worker -e ConnectionStrings:RabbitMq="amqp://rabbitmqhost:5672" debtors-worker 
```

#### Emails.Worker

```
docker build -t emails-worker -f Emails.Worker.Dockerfile .
docker rm emails-worker
docker run -d --network saga_net --name emails-worker -e ConnectionStrings:RabbitMq="amqp://rabbitmqhost:5672" emails-worker 
```