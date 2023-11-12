using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ISupermarketDAL
{
    Supermarket GetById(int id);
    void Insert(Supermarket supermarket);
    void Update(Supermarket supermarket);
    void Delete(int id);
    IEnumerable<Supermarket> GetAll();
    Supermarket GetSupermarketByTitle(string title);
}