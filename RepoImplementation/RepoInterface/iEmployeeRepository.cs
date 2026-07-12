using WebApplication1.Models;

namespace WebApplication1.RepoImplementation.RepoInterface
{
    public interface iEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee>UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        

    }
}
