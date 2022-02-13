using DLL.DBContext;                   
using DLL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DLL
{
    public static class  DllDependency
    {
        public static void AllDependency(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOpenIddict<int>()
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDatabaseDeveloperPageExceptionFilter();
            
            //repository dependency injection
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddTransient<IStudentRepository, StudentRepository>();
        }
    }
}