using BLL.Request;
using BLL.Services;
using DLL.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace BLL
{
    public static class BLLDependency
    {
        public static void AllDependency(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICourseStudentService, CourseStudentService>();
            
            AllFluentValidationDependency(services);
        }

        private static void AllFluentValidationDependency(IServiceCollection services)
        {
            services.AddTransient<IValidator<DepartmentInsertRequestViewModel>,
                DepartmentInsertRequestViewModelValidator>();
            
            services.AddTransient<IValidator<StudentInsertRequestViewModel>,
                StudentInsertRequestViewModelValidator>();
            
            services.AddTransient<IValidator<CourseInsertRequestViewModel>,
                CourseInsertRequestViewModelValidator>();
            
            services.AddTransient<IValidator<CourseAssignInsertViewModel>,
                CourseAssignInsertViewModelValidator>();
        }
    }
}


     