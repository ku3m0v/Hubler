﻿using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IEmployeeDAL
{
    // IEnumerable<Employee> FindByEmail(String email);
    Employee FindById(int id);
    int? Authenticate(string email, string passHash);
}