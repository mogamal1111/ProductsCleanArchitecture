using MediatR;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products.Queries.GetProducts;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;

    public GetProductsHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<List<ProductDto>> Handle(
        GetProductsQuery request, CancellationToken cancellationToken)
        => _repository.GetAllAsync(cancellationToken);
}
