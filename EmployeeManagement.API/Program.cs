using EmployeeManagement.API.DAL;
using EmployeeManagement.API.Repository;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddScoped<DepartmentRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<RoleRepository>();
            builder.Services.AddScoped<MessageRepository>();
            builder.Services.AddScoped<PostRepository>();
            builder.Services.AddScoped<FileDataRepository>();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
