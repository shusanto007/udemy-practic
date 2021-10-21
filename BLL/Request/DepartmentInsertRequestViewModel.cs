using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class DepartmentInsertRequestViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        
    }

    public class DepartmentInsertRequestViewModelValidator : AbstractValidator<DepartmentInsertRequestViewModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public DepartmentInsertRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3 )
                .MaximumLength(25).MustAsync(NameExists).WithMessage("Name Already Exists is System");
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(3)
                .MaximumLength(10).MustAsync(CodeExists).WithMessage("Code Already Exists is System");
        }

        private async Task<bool> CodeExists(string code , CancellationToken arg3)
        {
            if (string.IsNullOrEmpty(code))
            {
                return  true;
            }
            
            // Insert data
            var requiredService =  _serviceProvider.GetRequiredService<IDepartmentService>();
            return await requiredService.IsCodeExists(code);
        }

        private async Task<bool> NameExists(string name, CancellationToken arg2)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }

            // Insert data
            var requiredService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return await requiredService.IsNameExists(name);
        }
    }
}