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
        public  string Name { get; set; }

        public string Code { get; set; }
        
    }

    public class DepartmentInsertRequestViewModelValidator : AbstractValidator<DepartmentInsertRequestViewModel>
    {
       
        public DepartmentInsertRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3 )
                .MaximumLength(25).WithMessage("Name does not exists in our system");
            
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(3)
                .MaximumLength(10).WithMessage("Code does not exists in out system");
        }
    }
}