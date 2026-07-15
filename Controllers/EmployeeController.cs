using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.RepoImplementation.RepoInterface;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly iEmployeeRepository _repository;

        public EmployeeController(iEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _repository.GetEmployeesAsync();

            var result = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                Salary = e.Salary
            });

            return Ok(result);
        }

        [HttpPost("CreateEmployees")]
        public async Task<IActionResult> AddEmployees(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Salary = dto.Salary
            };

            var result = await _repository.AddEmployeeAsync(employee);

            return Ok(result);
        }

        [HttpPut("UpdateEmployees")]
        public async Task<IActionResult> UpdateEmployees(EmployeeDto dto)
        {
            var employee = new Employee
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                Salary = dto.Salary
            };

            var result = await _repository.UpdateEmployeeAsync(employee);

            return Ok(result);
        }

        [HttpDelete("DeleteEmployees/{id}")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            await _repository.DeleteEmployeeAsync(id);
            return Ok();
        }
    }
}