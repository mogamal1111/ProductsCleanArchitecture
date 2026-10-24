using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
