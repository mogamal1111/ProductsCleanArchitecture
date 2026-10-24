# Products API | Clean Architecture & CQRS

A RESTful Web API built with **ASP.NET Core 10**, following **Clean Architecture** principles and the **CQRS pattern** using MediatR.

The project demonstrates how to design a maintainable and well-structured backend application by separating business logic, application operations, data access, and API responsibilities.

## Technologies Used

- **ASP.NET Core 10** — RESTful Web API
- **C# / .NET 10** — Application development
- **Entity Framework Core** — Object-Relational Mapping
- **SQL Server** — Relational database
- **MediatR** — CQRS request handling
- **Dependency Injection** — Dependency management
- **Data Annotations** — Input validation
- **LINQ** — Database queries

---

## Architecture Overview

The solution follows Clean Architecture and is divided into four independent projects, each with a clearly defined responsibility.

| Layer | Responsibility |
|---|---|
| **Domain** | Core entities and business rules |
| **Application** | CQRS commands, queries, handlers, DTOs, and interfaces |
| **Infrastructure** | Database access, EF Core, and repository implementation |
| **API** | HTTP endpoints, request validation, and application configuration |

### Request Flow

An incoming HTTP request is processed through the following components:

**Controller → MediatR → Command/Query Handler → Repository → EF Core → SQL Server**

The controller handles the HTTP request, while MediatR dispatches the corresponding command or query to its handler.

Business logic and data access remain separated from the API layer.

---

## CQRS Implementation

The project applies **Command Query Responsibility Segregation (CQRS)** to separate operations that modify data from operations that retrieve data.

### Commands

**CreateProductCommand**

Creates a new product, validates its business rules, and saves it to the database.

Handled by `CreateProductHandler`.

### Queries

**GetProductsQuery**

Retrieves all products from the database.

Handled by `GetProductsHandler`.

**GetProductByIdQuery**

Retrieves a single product using its unique identifier.

Handled by `GetProductByIdHandler`.

MediatR connects each request to its corresponding handler, keeping controllers focused on HTTP responsibilities.

---

## API Endpoints

The application provides three RESTful endpoints.

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/products` | Retrieve all products |
| `GET` | `/api/products/{id}` | Retrieve a product by ID |
| `POST` | `/api/products` | Create a new product |

### Create Product

**POST** `/api/products`

Request Body:

```json
{
  "name": "Keyboard",
  "price": 199.99
}
```

Successful Response — `201 Created`:

```json
{
  "id": 1,
  "name": "Keyboard",
  "price": 199.99
}
```

### Get All Products

**GET** `/api/products`

Example Response — `200 OK`:

```json
[
  {
    "id": 1,
    "name": "Keyboard",
    "price": 199.99
  }
]
```

### Get Product By ID

**GET** `/api/products/1`

Returns the requested product or `404 Not Found` if the product does not exist.

---

## Validation & Error Handling

The API implements input validation and centralized exception handling.

Validation rules include:

- Product name is required.
- Product name must contain between 2 and 100 characters.
- Product price must be between `0.01` and `999999999.99`.
- Product IDs must be positive integers.

The application uses appropriate HTTP status codes:

| Status Code | Meaning |
|---|---|
| `200 OK` | Request completed successfully |
| `201 Created` | Product created successfully |
| `400 Bad Request` | Invalid request data |
| `404 Not Found` | Requested product does not exist |
| `500 Internal Server Error` | Unexpected server error |

The Domain layer also enforces business rules independently of HTTP request validation.

---

## Database & Data Access

The project uses **Entity Framework Core with SQL Server**.

Data access is implemented through the Repository Pattern.

`IProductRepository` defines the data access contract within the Application layer, while `ProductRepository` provides its implementation in the Infrastructure layer.

Read operations use:

- `AsNoTracking()` for efficient read-only queries.
- `Select()` to project entities into DTOs.
- Asynchronous methods for database operations.

During development, the database and Products table are automatically initialized using `EnsureCreatedAsync()`.

This initialization is intended for the educational project. A production application should use properly managed EF Core migrations.

---

## Getting Started

### Prerequisites

Make sure you have installed:

- .NET 10 SDK
- Visual Studio Code or Visual Studio
- SQL Server or SQL Server Express LocalDB
- Git (optional)

### 1. Clone the Repository

```bash
git clone https://github.com/mogamal1111/ProductsCleanArchitecture.git
```

Navigate to the project:

```bash
cd ProductsCleanArchitecture
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Database Connection

Open:

`Products.API/appsettings.json`

Configure the `DefaultConnection` connection string for your SQL Server instance.

Example using SQL Server LocalDB:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ProductsCleanArchitectureDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

If your LocalDB instance has a different name, replace `MSSQLLocalDB` with that name.

Do not commit database credentials or other secrets to Git.

### 4. Build the Solution

```bash
dotnet build
```

### 5. Run the Application

```bash
dotnet run --project Products.API --launch-profile http
```

The application starts at:

**http://localhost:5067**

### 6. Test the API

Open the following URL in your browser:

`http://localhost:5067/api/products`

You can also test the endpoints using Postman, PowerShell, or the included `Products.API.http` file.

Example PowerShell request:

```powershell
Invoke-RestMethod `
  -Uri "http://localhost:5067/api/products" `
  -Method Post `
  -ContentType "application/json" `
  -Body '{"name":"Keyboard","price":199.99}'
```

---

## Design Principles

This project demonstrates several important software engineering concepts:

**Separation of Concerns**

Each architectural layer has a clearly defined responsibility.

**Dependency Inversion**

Application logic depends on abstractions rather than database-specific implementations.

**CQRS**

Commands and queries are handled independently, improving code organization.

**Dependency Injection**

Application services and repositories are registered through the built-in .NET dependency injection container.

**Maintainability**

The modular structure makes the application easier to understand, extend, and maintain.

---

## Project Scope

This project was developed as a practical implementation of **ASP.NET Core Clean Architecture and CQRS concepts**.

It focuses on demonstrating architectural design, database integration, command/query separation, input validation, and RESTful API development.

The current implementation includes product creation and retrieval operations.

---

**Built with ASP.NET Core, Clean Architecture, CQRS, MediatR, and Entity Framework Core.**
