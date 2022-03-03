using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using Utility;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<Course> InsertAsync(CourseInsertRequestViewModel request);
        Task<List<Course>> GetAllAsync();

        Task<Course> DeleteAsync(string code);
        Task<Course> GetAAsync(string code);
        Task<Course> UpdateAsync(string code, Course course);

        Task testing(RequestMaker loginUser);
    }

    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Course> InsertAsync(CourseInsertRequestViewModel request)
        {

            var alreadyCourseInOurSystem = await _unitOfWork.CourseRepository
                .FindSingleAsync(x => 
                x.Code == request.Code ||
                x.Name == request.Name);

            if (alreadyCourseInOurSystem != null)
            {
                throw new ApplicationValidationException("Course code or name already in our system");
            }
            var course = new Course
            {
                Name = request.Name,
                Code = request.Code,
                Credit = request.Credit
            };
            await _unitOfWork.CourseRepository.CreateAsync(course);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;
            }
            throw new ApplicationValidationException("Course Insert failed");
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _unitOfWork.CourseRepository.GetList();
        }

        public async Task<Course> DeleteAsync(string code)
        {
            var course = await _unitOfWork.CourseRepository
                .FindSingleAsync(x => x.Code == code);
            
            if (course == null)
            {
                throw new ApplicationValidationException("Course NOt Found");
            }

            _unitOfWork.CourseRepository.Delete(course);
            
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return course;
            }
            throw new ApplicationValidationException("Some Problem in Deleting Data");
        }

        public async Task<Course> GetAAsync(string code)
        {
            var course =  await _unitOfWork.CourseRepository
                .FindSingleAsync(x => x.Code == code);
            
            if (course == null)
            {
                throw new ApplicationValidationException("Course not Found");
            }

            return course;
        }

        public async Task<Course> UpdateAsync(string code, Course aCourse)
        {
            var course = await _unitOfWork.CourseRepository
                .FindSingleAsync(x => x.Code == code);
            
            if (course == null)
            {
                throw new ApplicationValidationException("Course NOt Found");
            }
            /////////////////////////  CODE Check //////////////////////
            
            if (!string.IsNullOrWhiteSpace(aCourse.Code))
            {
                var codeAlreadyExists = await _unitOfWork.CourseRepository
                    .FindSingleAsync(x => x.Code == code);
                
                if (codeAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Code Has Already Been Updated");
                }

                course.Code = aCourse.Code;
            }
            /////////////////////////  NAME Check //////////////////////
            
            if (!string.IsNullOrWhiteSpace(aCourse.Name))
            {
                var nameAlreadyExists = await _unitOfWork.CourseRepository
                    .FindSingleAsync(x => x.Name == aCourse.Name);
                
                if (nameAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Name Has Already Been Updated");
                }

                course.Name = aCourse.Name;
            }
            
            // After the check update the department
            
            _unitOfWork.CourseRepository.UpdateAsync(course);
             
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return aCourse;
            }
            throw new ApplicationValidationException("Course Insert failed");
        }

        public  Task testing(RequestMaker loginUser)
        {
            var userId = loginUser.Principal.GetUserId();
            var userName = loginUser.Principal.GetUserName();
            var userRoll = loginUser.Principal.GetUserRoll();

            throw new NotImplementedException();
        }
    }
}