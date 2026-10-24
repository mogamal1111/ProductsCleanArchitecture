using Microsoft.EntityFrameworkCore;
using Products.Application.DTOs;
using Products.Application.Interfaces;
using Products.Domain.Entities;
using Products.Infrastructure.Persistence;

namespace Products.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken)
        => _dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Id)
            .Select(product => new ProductDto(product.Id, product.Name, product.Price))
            .ToListAsync(cancellationToken);

    public Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => _dbContext.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductDto(product.Id, product.Name, product.Price))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> AddAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}
