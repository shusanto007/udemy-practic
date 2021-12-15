using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Model;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        Task<Department> InsertAsync(DepartmentInsertRequestViewModel request);
        Task<List<Department>> GetAllAsync();

        Task<Department> DeleteAsync(string code);
        Task<Department> GetAAsync(string code);
        Task<Department> UpdateAsync(string code, Department department);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Department> InsertAsync(DepartmentInsertRequestViewModel request)
        {
            var alreadyStudentInSystem = await _unitOfWork.DepartmentRepository
                .FindSingleAsync(x =>
                x.Code == request.Code ||
                x.Name == request.Name);
            
            if (alreadyStudentInSystem != null)
            {
                throw new ApplicationValidationException("Department code or name already in our system");
            }
            
            var department = new Department
            {
                Name = request.Name,
                Code = request.Code
            };
            await _unitOfWork.DepartmentRepository.CreateAsync(department);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return department;
            }
            throw new ApplicationValidationException("Department Insert failed");
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _unitOfWork.DepartmentRepository.GetList();
        }

        public async Task<Department> DeleteAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository
                .FindSingleAsync(x => x.Code == code);
            
            if (department == null)
            {
                throw new ApplicationValidationException("Department NOt Found");
            }

            _unitOfWork.DepartmentRepository.Delete(department);
            
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return department;
            }
            throw new ApplicationValidationException("Some Problem in Deleting Data");
        }

        public async Task<Department> GetAAsync(string code)
        {
            var department =  await _unitOfWork.DepartmentRepository
                .FindSingleAsync(x => x.Code == code);
            
            if (department == null)
            {
                throw new ApplicationValidationException("Department not Found");
            }

            return department;
        }

        public async Task<Department> UpdateAsync(string code, Department adepartment)
        {
            var department = await _unitOfWork.DepartmentRepository
                .FindSingleAsync(x => x.Code == code);
            
            if (department == null)
            {
                throw new ApplicationValidationException("Department NOt Found");
            }
            /////////////////////////  CODE Check //////////////////////
            
            if (!string.IsNullOrWhiteSpace(adepartment.Code))
            {
                var codeAlreadyExists = await _unitOfWork.DepartmentRepository
                    .FindSingleAsync(x => x.Code == code);
                
                if (codeAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Code Has Already Been Updated");
                }

                department.Code = adepartment.Code;
            }
            /////////////////////////  NAME Check //////////////////////
            
            if (!string.IsNullOrWhiteSpace(adepartment.Name))
            {
                var nameAlreadyExists = await _unitOfWork.DepartmentRepository
                    .FindSingleAsync(x => x.Name == adepartment.Name);
                
                if (nameAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Name Has Already Been Updated");
                }

                department.Name = adepartment.Name;
            }
            
            // After the check update the department
            
            _unitOfWork.DepartmentRepository.UpdateAsync(department);
             
            if (await _unitOfWork.SaveCompletedAsync())
            {
                return adepartment;
            }
            throw new ApplicationValidationException("Department Insert failed");
        }
    }
}