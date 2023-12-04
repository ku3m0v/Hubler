using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ILkProductDAL
{
    LkProduct GetById(int id);
    void Insert(LkProduct lkProduct);
    void Update(LkProduct lkProduct);
    void Delete(int id);
    IEnumerable<LkProduct> GetAll();
    LkProduct GetByTitle(string title);
}