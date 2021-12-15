using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using DLL.ResponseViewModel;
using Utility.Exceptions;
using Utility.Models;

namespace BLL.Services
{
    public interface ICourseStudentService
    {
        Task<ApiSuccessResponse> InsertAsync(CourseAssignInsertViewModel request);
        Task<List<CourseStudent>> GetAllAsync();
        Task<StudentCourseViewModel> CourseListAsync(int studentId);
    }

    public class CourseStudentService : ICourseStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseStudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiSuccessResponse> InsertAsync(CourseAssignInsertViewModel request)
        {
            var isStudentAlreadyEnrolled = await _unitOfWork.CourseStudentRepository.FindSingleAsync(x =>
                x.CourseId == request.CourseId &&
                x.StudentId == request.StudentId);

            if (isStudentAlreadyEnrolled != null)
            {
                throw new ApplicationValidationException("Student Already present in this Course");
            }

            var courseStudent = new CourseStudent()
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId
            };

            await _unitOfWork.CourseStudentRepository.CreateAsync(courseStudent);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return new ApiSuccessResponse()
                {
                    StatusCode = 200,
                    Message = "Student Enroll SuccessFully"
                };
            }
            
            throw new ApplicationValidationException("Something went wrong with Enrollment");
        }
        
        public async Task<List<CourseStudent>> GetAllAsync()
        {
            return await _unitOfWork.CourseStudentRepository.GetList();
        }

        public async Task<StudentCourseViewModel> CourseListAsync(int studentId)
        {
            return await _unitOfWork.StudentRepository.GetSpecificStudentCourseList(studentId);
        }
    }
}
