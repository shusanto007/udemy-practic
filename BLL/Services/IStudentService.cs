using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using Utility.Exceptions;
using Utility.Models;

namespace BLL.Services
{
    public interface IStudentService
    {
        Task<Student> InsertAsync(StudentInsertRequestViewModel request);
        Task<List<Student>> GetAllAsync();
        Task<Student> GetAAsync(string email);
        Task<Student> DeleteAsync(string email);
        Task<Student> UpdateAsync(string email, Student student);

    }

    public  class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> InsertAsync(StudentInsertRequestViewModel request)
        {
            var studentAlreadyExists = await _unitOfWork.StudentRepository.FindSingleAsync(x =>
                x.Email == request.Email ||
                x.DepartmentId == request.DepartmentId);
            
            if (studentAlreadyExists != null)
            {
                throw new ApplicationValidationException("Student already in System ");
            }
            
            var student = new Student()
            {
                Name = request.Name,
                Email = request.Email,
                DepartmentId = request.DepartmentId
            };
            await _unitOfWork.StudentRepository.CreateAsync(student);

            if (await  _unitOfWork.SaveCompletedAsync())
            {
                return student;
            }
            throw new ApplicationValidationException("Student Insert failed");
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _unitOfWork.StudentRepository.GetList();
        }

        public async Task<Student> GetAAsync(string email)
        {
            var student = await _unitOfWork
                .StudentRepository.FindSingleAsync(x => x.Email == email);
            
            if (student == null)
            {
                throw new ApplicationValidationException("Department not Found");
            }

            return student;
        }

        public async Task<Student> DeleteAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository
                .FindSingleAsync(x => x.Email == email);
            
            if (student == null)
            {
                throw new ApplicationValidationException("Student nOt Found");
            }
            
            _unitOfWork.StudentRepository.Delete(student);
            
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return student;
            }
            throw new ApplicationValidationException("Some Problem in Deleting Data");
        }

        public async Task<Student> UpdateAsync(string email, Student astudent)
        {
            var student = await _unitOfWork.
                StudentRepository.FindSingleAsync(x => x.Email == email);
            
            if (student == null)
            {
                throw new ApplicationValidationException("Student  NOt Found");
            }
            
            /////////////////////////  Email Check //////////////////////
            
            if (!string.IsNullOrWhiteSpace(astudent.Email))
            {
                var emailAlreadyExists = await _unitOfWork.StudentRepository
                    .FindSingleAsync(x => x.Email == email);
                
                if (emailAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Email Has Already Been Updated");
                }

                student.Email = astudent.Email;
            }
            
            /////////////////////////  Name Check //////////////////////
            
            if (!string.IsNullOrWhiteSpace(astudent.Name))
            {
                var nameAlreadyExists = await _unitOfWork.StudentRepository
                    .FindSingleAsync(x => x.Name == astudent.Name);
                
                if (nameAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Name Has Already Been Updated");
                }

                student.Name = astudent.Name;
            }
            
            /////////////////////////  Update Student //////////////////////
            _unitOfWork.StudentRepository.UpdateAsync(student);

            if (await  _unitOfWork.SaveCompletedAsync())
            {
                return astudent;
            }
            
            throw new ApplicationValidationException("Department Insert failed");
        }
    }
}