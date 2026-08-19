using Inventory.Api.Data;
using Inventory.Api.DTOs;
using Inventory.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly InventoryDbContext _dbContext;

    public ProductController(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Code)
            .Select(product => new ProductResponse(
                product.Id,
                product.Code,
                product.Description,
                product.Balance,
                product.CreatedAt
            ))
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "Produto nao encontrado."
            });
        }

        return Ok(ToResponse(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request)
    {
        var normalizedCode = request.Code.Trim().ToUpperInvariant();

        var codeAlreadyExists = await _dbContext.Products
            .AnyAsync(product => product.Code == normalizedCode);

        if (codeAlreadyExists)
        {
            return Conflict(new
            {
                message = "Ja existe um produto com esse codigo."
            });
        }

        var product = new Product
        {
            Code = normalizedCode,
            Description = request.Description.Trim(),
            Balance = request.Balance,
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById), 
            new { id = product.Id }, 
            ToResponse(product)
        );
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Code,
            product.Description,
            product.Balance,
            product.CreatedAt
        );
    }
}