using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hubler.Controllers;

[ApiController]
[Route("api/non-perishable")]
public class NonPerishableController : ControllerBase
{
    private readonly IProductDAL _productDAL;
    private readonly INonPerishableDAL _nonPerishableDAL;
    
    public NonPerishableController(IProductDAL productDAL, INonPerishableDAL nonPerishableDAL)
    {
        _productDAL = productDAL;
        _nonPerishableDAL = nonPerishableDAL;
    }
    
    // GET: api/{type}/list
    [HttpGet("list"), Authorize]
    public IActionResult GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "admin" || role == "manager")
        {
            var products = _productDAL.GetAll();
            var nonPerishableProducts = new List<NonPerishableProductModel>();

            foreach (var product in products)
            {
                if (product.ProductType == "NP")
                {
                    var nonPerishable = _nonPerishableDAL.GetByProductId(product.Id);
                    nonPerishableProducts.Add(new NonPerishableProductModel
                    {
                        ProductId = product.Id,
                        Title = product.Title,
                        CurrentPrice = product.CurrentPrice,
                        ProductType = product.ProductType,
                        ShelfLife = nonPerishable.ShelfLife
                    });
                }
            }

            return Ok(nonPerishableProducts);
        }

        return BadRequest("You do not have permission to view this resource.");
    }
    
    // POST: api/{type}/insert
    [HttpPost("insert")]
    public IActionResult Insert(NonPerishableProductModel model)
    {
        if(model == null)
        {
            return BadRequest("Invalid model.");
        }

        var product = new Product
        {
            Title = model.Title,
            CurrentPrice = model.CurrentPrice,
            ProductType = model.ProductType
        };

        var productId = _productDAL.Insert(product);

        var nonPerishable = new NonPerishable
        {
            ProductId = productId,
            ShelfLife = model.ShelfLife
        };

        _nonPerishableDAL.Insert(nonPerishable);

        return Ok();
    }
    
    
    // GET: api/{type}/detail
    [HttpGet("detail")]
    public IActionResult GetById(int id)
    {
        var product = _productDAL.GetById(id);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        if (product.ProductType == "NP")
        {
            var nonPerishable = _nonPerishableDAL.GetByProductId(product.Id);
            return Ok(new NonPerishableProductModel
            {
                ProductId = product.Id,
                Title = product.Title,
                CurrentPrice = product.CurrentPrice,
                ProductType = product.ProductType,
                ShelfLife = nonPerishable.ShelfLife
            });
        }

        return BadRequest("Product is not non-perishable.");
    }
    
    // POST: api/{type}/update
    [HttpPost("update")]
    public IActionResult Update(NonPerishableProductModel model)
    {
        if(model == null)
        {
            return BadRequest("Invalid model.");
        }

        var product = _productDAL.GetById(model.ProductId);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        if (product.ProductType == "NP")
        {
            var nonPerishable = _nonPerishableDAL.GetByProductId(product.Id);
            nonPerishable.ShelfLife = model.ShelfLife;
            _nonPerishableDAL.Update(nonPerishable);
            return Ok();
        }

        return BadRequest("Product is not non-perishable.");
    }
    
    // DELETE: api/{type}/delete
    [HttpDelete("delete")]
    public IActionResult Delete(int id)
    {
        var product = _productDAL.GetById(id);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        if (product.ProductType == "NP")
        {
            _nonPerishableDAL.Delete(id);
            _productDAL.Delete(id);
            return Ok();
        }

        return BadRequest("Product is not non-perishable.");
    }
}