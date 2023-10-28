using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IProductDAL
{
    Product GetById(int id);
    void Insert(Product item);
    void Update(Product item);
    void Delete(int id);
    IEnumerable<Product> GetAll();
}