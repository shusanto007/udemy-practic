using System;
using System.Collections.Generic;
using DLL.Model;

namespace DLL.ResponseViewModel
{
    public class StudentCourseViewModel
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public List<Course> Courses { get; set; } = new List<Course>();
        
        public DateTimeOffset CreatAt { get; set; }
        public DateTimeOffset UpdateAt { get; set; }
    }
}