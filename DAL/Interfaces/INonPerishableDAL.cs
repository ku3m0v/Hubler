using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface INonPerishableDAL
{
    NonPerishable GetByProductId(int productId);
    void Insert(NonPerishable item);
    void Update(NonPerishable item);
    void Delete(int productId);
    IEnumerable<NonPerishable> GetAll();
}