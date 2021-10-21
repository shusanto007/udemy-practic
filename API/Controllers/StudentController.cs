using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using DLL.Model;
using DLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentController : MainApiController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET
        //[FromQuery] string rollnumber, [FromQuery] string nickName
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _studentService.GetAllAsync());
        }

        [HttpGet("{Email}")]
        public async Task<IActionResult> GetA(string email)
        {
            return Ok(await _studentService.GetAAsync(email));
        }

        //[FromForm] Student student
        [HttpPost]
        public async Task<IActionResult> Insert(Student student)
        {
            return Ok(await _studentService.InsertAsync(student));
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, Student student)
        {
            return Ok(await _studentService.UpdateAsync(email, student));
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            return Ok(await _studentService.DeleteAsync(email));
        }
    }
    
    
    public static class StudentStatic
    {
        private static List<Student> AllStudent { get; set; } = new List<Student>();

        public static Student InsertStudent(Student student)
        {
            AllStudent.Add(student);
            return student;
        }

        public static List<Student> GetAllStudent()
        {
            return AllStudent;
        }

        public static Student GetAStudent(string email)
        {
            return AllStudent.FirstOrDefault(x => x.Email == email);
        }

        public static Student UpdateStudent(string email, Student student)
        {
            var result = new Student();
            foreach (var aStudent in AllStudent)
            {
                if (email == aStudent.Email)
                {
                    aStudent.Name = student.Name;
                    result = aStudent;
                }
            }

            return result;
        }

        public static Student DeleteStudent(string email)
        {
            var student = AllStudent.FirstOrDefault(x => x.Email == email);
            AllStudent = AllStudent.Where(x => x.Email != student.Email).ToList();

            return student;
        }
    }
    
}