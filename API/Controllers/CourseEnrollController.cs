using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CourseEnrollController : MainApiController
    {
        private readonly ICourseStudentService _courseStudent;

        // GET
        public CourseEnrollController(ICourseStudentService courseStudent)
        {
            _courseStudent = courseStudent;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _courseStudent.GetAllAsync());
        }
        
        [HttpPost]
        public async Task<IActionResult> Insert(CourseAssignInsertViewModel request)
        {
            return Ok(await _courseStudent.InsertAsync(request));
        }
        
        [HttpGet("{studentId}")]
        public async Task<IActionResult> CourseList(int studentId)
        {
            return Ok(await _courseStudent.CourseListAsync(studentId));
        }
    }
    
}