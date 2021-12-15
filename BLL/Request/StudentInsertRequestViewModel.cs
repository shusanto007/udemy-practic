using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class StudentInsertRequestViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
    
    public class StudentInsertRequestViewModelValidator : AbstractValidator<StudentInsertRequestViewModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public StudentInsertRequestViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;


            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(3)
                .MaximumLength(50).WithMessage("Name can not be empty");
            
            RuleFor(x => x.Email).NotNull().NotEmpty().MinimumLength(3)
                .EmailAddress().WithMessage("Email Already Exists is System");
            
            RuleFor(x => x.DepartmentId).GreaterThan(0)
                .WithMessage("Department Not in Our System");
        }
    }
}