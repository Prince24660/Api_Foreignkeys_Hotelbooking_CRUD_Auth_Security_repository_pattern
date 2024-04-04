using API_Foreignkey.IReposiory;
using API_Foreignkey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Foreignkey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="User")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]   
        public List<Students> GetStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        [HttpPost]
        [Authorize]
        public Students AddStudents([FromBody] Students std)
        {
            var addstd = _studentRepository.AddStudent(std);
            return addstd;
        }
    }
}
