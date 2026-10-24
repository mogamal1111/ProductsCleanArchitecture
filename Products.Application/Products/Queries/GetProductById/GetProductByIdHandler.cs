using MediatR;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<ProductDto?> Handle(
        GetProductByIdQuery request, CancellationToken cancellationToken)
        => _repository.GetByIdAsync(request.Id, cancellationToken);
}
