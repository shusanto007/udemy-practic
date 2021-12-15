using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseInsertRequestViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Credit { get; set; }
        
    }

    public class CourseInsertRequestViewModelValidator : AbstractValidator<CourseInsertRequestViewModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public CourseInsertRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3 )
                .MaximumLength(25).WithMessage("Name Already Exists is System");
            RuleFor(x => x.Code).NotNull().NotEmpty().MinimumLength(3)
                .MaximumLength(10).WithMessage("Code Already Exists is System");
            RuleFor(x => x.Credit).NotEmpty().NotNull();
        }

        
    }
}