using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion( "1.0" )]
    [ApiController]
    [Route("api/{version:apiVersion}/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // GET ALL DATA
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentRepository.GetAllAsync());
        }

        // GET ONE DATA
        [HttpGet("{code}")]
        public async Task<IActionResult> GetA(string code)
        {
            return Ok(await _departmentRepository.GetAAsync(code));
        }

        //INSERT NEW DATA
        [HttpPost]
        public async Task<IActionResult> Insert(Department department)
        {
            return Ok(await _departmentRepository.InsertAsync(department));
        }

        //UPDATE ONE DATA
        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, Department department)
        {
            return Ok(await _departmentRepository.UpdateAsync(code, department));
        }
        
        //DELETE ONE DATA
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            return Ok(await _departmentRepository.DeleteAsync(code));
        }
        
        
    }
}