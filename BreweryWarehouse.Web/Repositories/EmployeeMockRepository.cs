using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Repositories;

public class EmployeeMockRepository
{
    private static readonly List<Employee> Employees;

    static EmployeeMockRepository()
    {
        DataSeeder.Seed(
            out _,
            out _,
            out _,
            out _,
            out _,
            out List<Employee> employees);

        Employees = employees;
    }

    public List<Employee> GetAll()
    {
        return Employees;
    }

    public Employee? GetById(int id)
    {
        return Employees.FirstOrDefault(employee => employee.Id == id);
    }
}
