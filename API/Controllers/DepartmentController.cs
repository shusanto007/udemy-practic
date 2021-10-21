using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
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
        private readonly IDepartmentService _departmentService;
        
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET ALL DATA
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAllAsync());
        }

        // GET ONE DATA
        [HttpGet("{code}")]
        public async Task<IActionResult> GetA(string code)
        {
            return Ok(await _departmentService.GetAAsync(code));
        }

        //INSERT NEW DATA
        [HttpPost]
        public async Task<IActionResult> Insert(DepartmentInsertRequestViewModel request)
        {
            return Ok(await _departmentService.InsertAsync(request));
        }

        //UPDATE ONE DATA
        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, Department department)
        {
            return Ok(await _departmentService.UpdateAsync(code, department));
        }
        
        //DELETE ONE DATA
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            return Ok(await _departmentService.DeleteAsync(code));
        }
        
        
    }
}