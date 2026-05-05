using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Repositories;

public class EmployeeRepository
{
    private readonly BreweryWarehouseDbContext _context;

    public EmployeeRepository(BreweryWarehouseDbContext context)
    {
        _context = context;
    }

    public List<Employee> GetAll()
    {
        return _context.Employees.ToList();
    }

    public Employee? GetById(int id)
    {
        return _context.Employees.FirstOrDefault(employee => employee.Id == id);
    }
}
