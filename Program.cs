
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using WebApplication1.RepoImplementation;
using WebApplication1.RepoImplementation.RepoInterface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<iEmployeeRepository, EmployeeRepository>();

builder.Services.AddOpenApi();

// 👇 ADD THIS - CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularApp", policy =>
//    {
//        policy.WithOrigins("http://localhost:4200")
//      .AllowAnyMethod()
//      .AllowAnyHeader();
//    });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
           "https://employee-management-system-123.netlify.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 👇 ADD THIS - must be before UseAuthorization, after UseHttpsRedirection
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();