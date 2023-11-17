using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ILkRoleDAL
{
    LkRole GetById(int id);
    LkRole GetByRoleName(string roleName);
    void Insert(LkRole role);
    void Update(LkRole role);
    void Delete(int id);
    IEnumerable<LkRole> GetAll();
}