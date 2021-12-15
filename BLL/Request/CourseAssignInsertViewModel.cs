using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseAssignInsertViewModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
    
    public class CourseAssignInsertViewModelValidator : AbstractValidator<CourseAssignInsertViewModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public CourseAssignInsertViewModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.StudentId).NotNull()
                .NotEmpty().WithMessage("Student does not exists in our system");
            
            RuleFor(x => x.CourseId).NotNull()
                .NotEmpty().WithMessage("Course does not exists in out system");
        }
    }
}
