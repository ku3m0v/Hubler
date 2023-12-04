using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IViewDAL<T>
{
    IEnumerable<T> GetAll();
}