using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.ModelDTO;
using WebServer.WebServer.Model.Domain;
using WebServer.WebServer.Model.Domain.Entities;

namespace WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly DomainDbContext _db;

    public ProductController(DomainDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all products (items) with basic info.
    /// </summary>
    [HttpGet("getAllProduct")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> GetAllProduct()
    {
        var products = await _db.Titems
            .AsNoTracking()
            .Select(x => new ProductDto(x.Id, x.Sku, x.Name))
            .ToListAsync();

        return Ok(products);
    }
}
