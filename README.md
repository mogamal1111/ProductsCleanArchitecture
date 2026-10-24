# Products API — Clean Architecture + CQRS (ASP.NET Core)

A complete implementation of the **Task on page 15** of `ASP.NET_Core_Architecture.pdf`.

## Checklist

- [x] Product entity with `Id`, `Name`, and `Price`
- [x] `GET /api/products` (read all products)
- [x] `GET /api/products/{id}` (read by ID)
- [x] `POST /api/products` (create product)
- [x] 4 independent projects: `Domain`, `Application`, `Infrastructure`, `API`
- [x] `CQRS` with three requests/handlers and MediatR
- [x] `EF Core` using SQL Server
- [x] DTOs, Data Annotations / `IValidatableObject` validation
- [x] Dependency Injection and clean dependency directions

## Architecture

```text
Products.API  ------->  Products.Application  -------> Products.Domain
     |                         ^
     +---> Products.Infrastructure --+----------------> Products.Domain
```

- **Domain**: `Product` entity and its invariants. Does not reference external layers.
- **Application**: CQRS commands, queries, handlers, `ProductDto`, `IProductRepository`.
- **Infrastructure**: EF Core `ProductsDbContext`, SQL Server, `ProductRepository` implementation.
- **API**: HTTP controller, input DTO validation, startup + dependency registration.
- **Runtime flow**: HTTP -> Controller -> ISender -> MediatR Handler -> Repository -> EF Core -> SQL Server.

## Requirements

- .NET **10 SDK** (project targets `net10.0`)
- Microsoft SQL Server (Windows **SQL Server LocalDB** is the default connection)
- Internet access for the first `dotnet restore` to download NuGet packages.

## Run on Windows with Visual Studio Code

1. Extract the ZIP and open the **ProductsCleanArchitecture** folder in VS Code (`File > Open Folder`).
2. Open `Products.API/appsettings.json`. Default connection string:

   ```text
   Server=(localdb)\MSSQLLocalDB;Database=ProductsCleanArchitectureDb;Trusted_Connection=True;TrustServerCertificate=True;
   ```

3. If you use LocalDB, verify it is installed with PowerShell:

   ```powershell
   sqllocaldb info
   ```

   If LocalDB is unavailable, install SQL Server Express LocalDB or edit the connection string for your SQL Server instance (examples below).
4. From the **solution root** in VS Code Terminal:

   ```powershell
   dotnet restore
   dotnet run --project Products.API --launch-profile http
   ```

5. API listens on `http://localhost:5067`. In Development, **EF Core automatically creates the database and Products table** using `EnsureCreatedAsync` the first time the app starts.

## Test the API (PowerShell)

```powershell
# Add product: should return 201 Created, with Id
Invoke-RestMethod -Method Post -Uri http://localhost:5067/api/products -ContentType 'application/json' -Body '{"name":"Keyboard","price":199.99}'

# Read all products
Invoke-RestMethod http://localhost:5067/api/products

# Read one product
Invoke-RestMethod http://localhost:5067/api/products/1
```

Alternatively, open `Products.API/Products.API.http` with the VS Code **REST Client** extension (or copy its requests to Postman).

| Request | Expected result |
|---|---|
| `GET /api/products` | `200 OK`, JSON array |
| `GET /api/products/1` | `200 OK` + product, or `404 Not Found` |
| `POST /api/products` valid JSON | `201 Created`, new product and `Location` header |
| `POST /api/products` invalid name/price | `400 Bad Request`, validation errors |

Example `POST` JSON:

```json
{
  "name": "Laptop",
  "price": 27500.00
}
```

## SQL Server connection examples

`Products.API/appsettings.json` uses Windows LocalDB by default. If you already have SQL Server Express on your computer, you might use:

```text
Server=.\SQLEXPRESS;Database=ProductsCleanArchitectureDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Or for a server configured with SQL Server Authentication:

```text
Server=localhost,1433;Database=ProductsCleanArchitectureDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;
```

**Don't commit passwords.** Override the connection using user secrets, environment variables, or another secure configuration source instead of putting a real password in `appsettings.json`:

```powershell
cd Products.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=ProductsCleanArchitectureDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
cd ..
```

## CQRS implementation

- **Query** `GetProductsQuery` -> `GetProductsHandler` -> list projected as DTOs (`AsNoTracking`).
- **Query** `GetProductByIdQuery` -> `GetProductByIdHandler` -> matching product DTO or null.
- **Command** `CreateProductCommand` -> `CreateProductHandler` -> creates Domain entity and saves it -> returns product DTO.

All three handlers depend only on `IProductRepository`, not on EF Core.

## Validation

- HTTP request body: `CreateProductRequest` uses `Required`, `StringLength`, `Range`, and `IValidatableObject`.
- `[ApiController]` automatically returns `400 Bad Request` when it detects invalid model input.
- Domain `Product` independently enforces valid name and price so other callers cannot create an invalid entity.

## Important note on EF Core migrations

For this **training exercise** `EnsureCreatedAsync()` automatically initializes a fresh SQL Server database in Development; therefore you don't need a separate EF migration command to complete the assignment. **Do not mix `EnsureCreated` with `Database.Migrate` / EF migrations against the same database.** If this becomes a production project, replace the demo initialization with versioned EF Core migrations and start from a database created through migrations.

## Technologies

- .NET 10 / ASP.NET Core Web API
- MediatR 12.4.1
- EF Core SQL Server 10.0.0
- DataAnnotations and model validation
