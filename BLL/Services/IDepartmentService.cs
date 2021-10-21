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
        
        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Department> InsertAsync(DepartmentInsertRequestViewModel request)
        {
            Department adepartment = new Department();
            adepartment.Name = request.Name;
            adepartment.Code = request.Code;
            return await _departmentRepository.InsertAsync(adepartment);
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department> DeleteAsync(string code)
        {
            var department = await _departmentRepository.GetAAsync(code);
            if (department == null)
            {
                throw new ApplicationValidationException("Department NOt Found");
            }

            if (await _departmentRepository.DeleteAsync(department))
            {
                return department;
            }

            throw new ApplicationValidationException("Some Problem in Updating Data");


        }

        public async Task<Department> GetAAsync(string code)
        {
            var department =  await _departmentRepository.GetAAsync(code);
            if (department == null)
            {
                throw new ApplicationValidationException("Department not Found");
            }

            return department;
        }

        public async Task<Department> UpdateAsync(string code, Department adepartment)
        {
            var department = await _departmentRepository.GetAAsync(code);
            
            if (department == null)
            {
                throw new ApplicationValidationException("Department NOt Found");
            }

            if (!string.IsNullOrWhiteSpace(adepartment.Code))
            {
                var codeAlreadyExists = await _departmentRepository.FindByCode(adepartment.Code);
                if (codeAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Code Has Already Been Updated");
                }

                department.Code = adepartment.Code;
            }
            
            if (!string.IsNullOrWhiteSpace(adepartment.Name))
            {
                var nameAlreadyExists = await _departmentRepository.FindByName(adepartment.Name);
                if (nameAlreadyExists != null)
                {
                    throw new ApplicationValidationException("The Name Has Already Been Updated");
                }

                department.Name = adepartment.Name;
            }

            if (await _departmentRepository.UpdateAsync(department))
            {
                return department;
            }
            
            throw new ApplicationValidationException("Some Problem in Updating Data");
        }

        public async Task<bool> IsCodeExists(string code)
        {
            var department = await _departmentRepository.FindByCode(code);

            return department == null;
        }

        public async Task<bool> IsNameExists(string name)
        {                                                                          
            var department = await _departmentRepository.FindByName(name);

            if (department == null)
            {
                return true;
            }

            return false;
        }
    }
}