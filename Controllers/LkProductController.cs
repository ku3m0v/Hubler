using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hubler.Controllers;

[Route("api/lkproduct")]
[ApiController]
public class LkProductController : ControllerBase
{
    private readonly ILkProductDAL _lkProductDal;

    public LkProductController(ILkProductDAL lkProductDal)
    {
        _lkProductDal = lkProductDal;
    }

    // GET: api/lkproduct/list
    [HttpGet("list")]
    public ActionResult<IEnumerable<LkProduct>> GetAll()
    {
        var lkProducts = _lkProductDal.GetAll();
        return Ok(lkProducts);
    }

    // GET: api/lkproduct/get/{title}
    [HttpGet("get/{title}")]
    public ActionResult<LkProduct> GetByTitle(string title)
    {
        var lkProduct = _lkProductDal.GetByTitle(title);
        if (lkProduct == null)
        {
            return NotFound();
        }
        return Ok(lkProduct);
    }

    // POST: api/lkproduct/insert
    [HttpPost("insert")]
    public ActionResult Insert([FromBody] LkProduct lkProduct)
    {
        _lkProductDal.Insert(lkProduct);
        return Ok(new { Message = "Product inserted successfully."});
    }

    // POST: api/lkproduct/update
    [HttpPost("update")]
    public ActionResult Update([FromBody] LkProduct lkProduct)
    {
        var existingProduct = _lkProductDal.GetById(lkProduct.LkProductId);
        if (existingProduct == null)
        {
            return NotFound();
        }
        _lkProductDal.Update(lkProduct);
        return Ok(new { Message = "Product updated successfully." });
    }

    // DELETE: api/lkproduct/delete/{id}
    [HttpDelete("delete/{id}")]
    public ActionResult Delete(int id)
    {
        var lkProduct = _lkProductDal.GetById(id);
        if (lkProduct == null)
        {
            return NotFound();
        }
        _lkProductDal.Delete(id);
        return Ok(new { Message = "Product deleted successfully." });
    }
}