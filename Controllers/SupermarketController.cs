using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hubler.Controllers;

    [Route("api/supermarket")]
    [ApiController]
    public class SupermarketController : ControllerBase
    {
        private readonly ISupermarketDAL _supermarketDAL;
        private readonly IAddressDAL _addressDAL;

        public SupermarketController(ISupermarketDAL supermarketDAL, IAddressDAL addressDAL)
        {
            _supermarketDAL = supermarketDAL;
            _addressDAL = addressDAL;
        }
        
        [HttpGet("list")]
        public ActionResult<List<SupermarketWithAddressModel>> GetAll()
        {
            var supermarkets = _supermarketDAL.GetAll();
            var supermarketWithAddressModels = new List<SupermarketWithAddressModel>();

            foreach (var supermarket in supermarkets)
            {
                var address = _addressDAL.GetById(supermarket.AddressId);
                if (address != null)
                {
                    supermarketWithAddressModels.Add(new SupermarketWithAddressModel
                    {
                        Title = supermarket.Title,
                        Phone = supermarket.Phone,
                        // Address fields
                        Street = address.Street,
                        House = address.House,
                        City = address.City,
                        PostalCode = address.PostalCode,
                        Country = address.Country
                    });
                }
            }

            if (!supermarketWithAddressModels.Any())
            {
                return NotFound("No supermarkets found.");
            }
            return Ok(supermarketWithAddressModels);
        }

        [HttpPost("update")]
        public void Post([FromBody] SupermarketWithAddressModel model)
        {
            var newAddress = new Address
            {
                Street = model.Street,
                House = model.House,
                City = model.City,
                PostalCode = model.PostalCode,
                Country = model.Country
            };
            
            int addressId = _addressDAL.Insert(newAddress);
            
            var newSupermarket = new Supermarket
            {
                Title = model.Title,
                Phone = model.Phone,
                AddressId = addressId
            };
            _supermarketDAL.Insert(newSupermarket);
        }
        
        
        [HttpDelete("delete")]
        public void Delete(string title)
        {
            _supermarketDAL.Delete(title);
        }
        
    }