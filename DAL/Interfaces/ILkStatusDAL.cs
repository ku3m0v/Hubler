using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ILkStatusDAL
{
    LkStatus GetById(int id);
    void Insert(LkStatus status);
    void Update(LkStatus status);
    void Delete(int id);
    IEnumerable<LkStatus> GetAll();
}