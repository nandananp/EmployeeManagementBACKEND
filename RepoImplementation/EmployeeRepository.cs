using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.RepoImplementation.RepoInterface;

namespace WebApplication1.RepoImplementation
{
    public class EmployeeRepository:iEmployeeRepository
    {

        private readonly AppDbContext _dbcontext;

        public EmployeeRepository(AppDbContext context)
        {
            _dbcontext = context;

        }
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _dbcontext.Employees.ToListAsync();

        }
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _dbcontext.Employees.Add(employee);
            await _dbcontext.SaveChangesAsync();
            return employee;


        }

        public async Task<Employee>UpdateEmployeeAsync(Employee employee)
        {
            _dbcontext.Employees.Update(employee);
            await _dbcontext.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
          var employee =   await _dbcontext.Employees.FindAsync(id);
            if(employee  != null)
            {
                _dbcontext.Employees.Remove(employee);
                await _dbcontext.SaveChangesAsync();

            }
        }




        




    }
}
