using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.API.Contracts;
using Products.Application.DTOs;
using Products.Application.Products.Commands.CreateProduct;
using Products.Application.Products.Queries.GetProductById;
using Products.Application.Products.Queries.GetProducts;

namespace Products.API.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _sender.Send(new GetProductsQuery(), cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest(new ProblemDetails { Title = "Id must be greater than zero.", Status = 400 });

        var product = await _sender.Send(new GetProductByIdQuery(id), cancellationToken);
        if (product is null)
            return NotFound(new ProblemDetails { Title = "Product not found.", Status = 404 });

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        // [ApiController] runs DTO validation automatically before this action.
        var command = new CreateProductCommand(request.Name.Trim(), request.Price);
        var product = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
