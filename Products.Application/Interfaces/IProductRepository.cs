using Products.Application.DTOs;
using Products.Domain.Entities;

namespace Products.Application.Interfaces;

// The application depends on an abstraction, not on EF Core or SQL Server.
public interface IProductRepository
{
    Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(Product product, CancellationToken cancellationToken);
}
