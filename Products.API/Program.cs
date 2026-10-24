using Microsoft.EntityFrameworkCore;
using Products.Application;
using Products.Infrastructure;
using Products.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Centralized error handling; never expose raw exception details to API clients.
app.UseExceptionHandler();

app.MapControllers();

// A convenient zero-command database setup for this educational task.
// IMPORTANT: EnsureCreated is for this local demo only; do not mix it with EF migrations.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();
