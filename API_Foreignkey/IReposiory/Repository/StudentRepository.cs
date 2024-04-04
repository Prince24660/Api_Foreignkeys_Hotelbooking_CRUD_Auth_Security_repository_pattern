using API_Foreignkey.Models;
using Test2.Data;

namespace API_Foreignkey.IReposiory.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Students AddStudent(Students students)
        {
            var emp = _context.studentss.Add(students);
            _context.SaveChanges();
            return emp.Entity;
        }

        public List<Students> GetAllStudents()
        {
            var employees = _context.studentss.ToList();
            return employees;
        }
    }
}
