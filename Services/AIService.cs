using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;

        public AIService(HttpClient httpClient, AppDbContext dbContext)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }

        public async Task<string> AskQuestion(string question)
        {
            try
            {
                var employees = await _dbContext.Employees.ToListAsync();

                if (!employees.Any())
                {
                    return "No employees found in the database.";
                }

                string employeeContext = BuildEmployeeContext(employees);

                string prompt = $@"
You are an HR Assistant.

Here is our employee database:

{employeeContext}

Answer the user's question ONLY using the employee data above.

Question:
{question}
";

                var requestBody = new
                {
                    model = "qwen2.5:3b",
                    prompt = prompt,
                    stream = false
                };

                var json = JsonSerializer.Serialize(requestBody);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                string url = "http://localhost:11434/api/generate";

                var response = await _httpClient.PostAsync(url, content);

                var responseText = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseText);

                if (!response.IsSuccessStatusCode)
                {
                    return $"Ollama Error ({(int)response.StatusCode}): {responseText}";
                }

                using JsonDocument doc = JsonDocument.Parse(responseText);

                return doc.RootElement
                          .GetProperty("response")
                          .GetString() ?? "No response received.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string BuildEmployeeContext(List<Employee> employees)
        {
            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"ID: {emp.Id}");
                sb.AppendLine($"Name: {emp.Name}");
                sb.AppendLine($"Email: {emp.Email}");
                sb.AppendLine($"Department: {emp.Department}");
                sb.AppendLine($"Salary: {emp.Salary}");
                sb.AppendLine("--------------------------------");
            }

            return sb.ToString();
        }
    }
}