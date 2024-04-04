using API_Foreignkey.IReposiory;
using API_Foreignkey.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Foreignkey.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
       
        [HttpGet]
        public List<Students>GetStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        [HttpPost]
        public Students AddStudents ([FromBody]Students std)
        {
            var addstd=_studentRepository.AddStudent(std);
            return addstd;
        }

 
    }
}
