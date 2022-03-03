using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Model;
using DLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace API.Controllers
{
    public class CourseController : MainApiController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET ALL DATA
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _courseService.GetAllAsync());
        }

        // GET ONE DATA
        [HttpGet("{code}")]
        public async Task<IActionResult> GetA(string code)
        {
            return Ok(await _courseService.GetAAsync(code));
        }

        //INSERT NEW DATA
        [HttpPost]
        public async Task<IActionResult> Insert(CourseInsertRequestViewModel request)
        {
            return Ok(await _courseService.InsertAsync(request));
        }

        //UPDATE ONE DATA
        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, Course course)
        {
            return Ok(await _courseService.UpdateAsync(code, course));
        }
        
        //DELETE ONE DATA
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            return Ok(await _courseService.DeleteAsync(code));
        }
        
       [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("testing")]
        public async Task<IActionResult> Testing()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };

            await _courseService.testing(loginUser);
            
            return Ok("testing");
        }
        
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "admin")]
        [HttpGet("testing1")]
        public async Task<IActionResult> Testing1()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };
            return Ok("testing1");
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "manager")]
        [HttpGet("testing2")]
        public async Task<IActionResult> Testing2()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };
            return Ok("testing2");
        }
        
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "supervisor")]
        [HttpGet("testing3")]
        public async Task<IActionResult> Testing3()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };
            return Ok("testing3");
        }
        
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "admin,manager")]
        [HttpGet("testing4")]
        public async Task<IActionResult> Testing4()
        {
            var loginUser = new RequestMaker()
            {
                Principal = User
            };
            return Ok("testing4");
        }
        
    }
}