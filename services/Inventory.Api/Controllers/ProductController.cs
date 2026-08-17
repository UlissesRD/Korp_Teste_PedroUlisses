using Inventory.Api.DTOs;
using Inventory.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> Products = [];

    [HttpGet]
    public ActionResult<IEnumerable<ProductResponse>> GetAll()
    {
        var response = Products.Select(product => new ProductResponse(
            product.Id,
            product.Code,
            product.Description,
            product.Balance,
            product.CreatedAt
        ));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProductResponse> GetById(Guid id)
    {
        var product = Products.FirstOrDefault(product => product.Id == id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "Produto nao encontrado."
            });
        }

        return Ok(new ProductResponse(
            product.Id,
            product.Code,
            product.Description,
            product.Balance,
            product.CreatedAt
        ));
    }

    [HttpPost]
    public ActionResult<ProductResponse> Create(CreateProductRequest request)
    {
        var normalizedCode = request.Code.Trim().ToUpperInvariant();

        var codeAlreadyExists = Products.Any(
            product => product.Code == normalizedCode
        );

        if (codeAlreadyExists)
        {
            return Conflict(new
            {
                message = "Já existe um produto com esse código."
            });
        }

        var product = new Product
        {
            Code = normalizedCode,
            Description = request.Description.Trim(),
            Balance = request.Balance,
        };

        Products.Add(product);

        var response = new ProductResponse(
            product.Id,
            product.Code,
            product.Description,
            product.Balance,
            product.CreatedAt
        );

        return CreatedAtAction(
            nameof(GetById), 
            new { id = product.Id }, 
            response
        );
    }
}