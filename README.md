**Order API - Clean Architecture (.NET 8)**

**Overview**
This is a Clean Architecture-based ASP.NET Core Web API application built on .NET 8. It includes:

Authentication & Authorization using Identity + JWT
Order Management with In-Memory Queue + Background Worker
Persistence with Entity Framework Core (SQL Server)
Logging via Serilog
Validation via FluentValidation
Object Mapping via AutoMapper
Testing with xUnit
Global Exception Handling Middleware
Fully adheres to OOP and SOLID principles


Features

Auth Controller
Handles user registration and login.

POST /api/Auth/register
Registers a new user using ASP.NET Core Identity.

POST /api/Auth/login
Authenticates a user and returns a JWT token. This token is required for accessing protected Order APIs.

Order Controller
All endpoints require JWT authentication.

1. POST /api/Order/create-order
Creates a new order.

Orders are stored in an In-Memory Order Queue (acts like a messaging queue). A background job processes the messages and saves them to the database.

2. GET /api/Order/get-all-order?pageNumber=1&pageSize=10
Retrieves a list of processed orders.

Supports pagination using pageNumber and pageSize query parameters.

3. GET /api/Order/{id}
Retrieves a single order by ID.

Running the Application

Create Migration
Run the following command to create a migration for the database:
dotnet ef migrations add Order-migration --project .\Domain --startup-project .\OrderApi

Update Database
Update the database with the migration:
dotnet ef database update --project .\Domain --startup-project .\OrderApi

Register and Login
Use the POST /api/Auth/register endpoint to register a user.
Use the POST /api/Auth/login endpoint to login and get a JWT token.

Use the Token
Include the JWT token in the Authorization header for accessing the Order API endpoints.

Architecture & Design
The application follows Clean Architecture principles.
It adheres to SOLID principles and OOP concepts.
Serilog is used for logging.
AutoMapper is used for object mapping.
FluentValidation is used for validation.
xUnit is used for unit tests.
Global exception handling is managed by a custom middleware.


Technologies Used - .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server, IdentityServer & JWT for authentication & authorization,
Serilog for logging, FluentValidation for validation,AutoMapper for object mapping, xUnit for testing






