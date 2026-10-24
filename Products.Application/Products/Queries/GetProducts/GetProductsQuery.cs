using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Products.Queries.GetProducts;

// A query retrieves data without changing state.
public sealed record GetProductsQuery : IRequest<List<ProductDto>>;
