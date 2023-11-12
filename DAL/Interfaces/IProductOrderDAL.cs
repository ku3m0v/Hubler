using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IProductOrderDAL
{
    ProductOrder GetById(int id);
    void Insert(ProductOrder order);
    void Update(ProductOrder order);
    void Delete(int id);
    IEnumerable<ProductOrder> GetAll();
}