using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Products.Commands.CreateProduct;

// A command represents a state-changing operation.
public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<ProductDto>;
