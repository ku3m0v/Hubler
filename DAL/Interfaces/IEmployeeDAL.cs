using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IEmployeeDAL
{
    Employee GetById(int id);
    String Insert(Employee employee);
    void Update(Employee employee);
    void Delete(int id);
    IEnumerable<Employee> GetAll();
    Employee GetByEmail(string email);

}