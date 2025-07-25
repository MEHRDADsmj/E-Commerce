# E-Commerce .NET
This is an educational project focused on learning microservices and clean architecture through building an e-commerce app.


## Getting Started
Just clone the reposritory and start the services and use the E-Commerce postman collection. If you want to run the project using Docker, run the following command in the main directory of the project:
```markdown
  docker compose up --build
```
Then, using the E-Commerce postman collection, you can start testing the APIs. Keep it mind that for easier usage of the collections, you have to import Docker or Local environments accordingly.

## Project Structure
### User Service
User service applies CRUD operations on the database to manage and control user interactions. It utilizes PostgreSQL as the database and applies operations as commands using the mediator pattern in a CQRS style with the MediatR package.

### Product Service
Applies the same CRUD operations for products, allowing easy and fast modifications on each product. Products can be queried from the database in a paginated way to reduce performance overhead or implement lazy-loading in the client. Also, it 
provides an API to query products in batches, which I call "bulk query".

### Cart Service
Cart service manages and stores each user's shopping cart. The data is stored in memory for fast data access using Redis. It interacts with Order Service to create orders and start the payment workflow.

### Order Service
This service handles order creation and sends and receives events about payment. When an order is created, the service emits an event named "Order Created Event" to Payment Service to notify that a user wants to make a payment.
It fetches the info from Cart and Product services to calculate the total price of the order and finally saves the order in the database to keep a record of each user's orders.

### Payment Service
Payment Service is the only service that doesn't expose any APIs to make HTTP requests. Instead, it is an event-driven service that listens to a message queue (in this case RabbitMQ) and reacts to the events. When an order is created, this service
processes the payment and returns a result of Success or Failure and publishes an event to notify Order Service about the status of the order.

### Shared
This folder contains common and shared classes and interfaces that are used in every other service, such as classes and structures for returning results of a use case handler class in an organized way.

### Tests
This folder holds all the tests, including unit and integration tests, written in xUnit, Moq, Bogus, TestContainers, etc.

### nginx
This folder keeps the configuration files for Nginx which has the responsibility of load balancing and sending each request to its corresponding service.

## Technologies Used
### .NET 8, PostgreSQL, Redis RabbitMQ, xUnit, Nginx, Docker, Entity Framework, WebAPI, Postman, Swagger
