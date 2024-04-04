using API_Foreignkey.Models;

namespace API_Foreignkey.IReposiory
{
    public interface IStudentRepository
    {
        public List<Students> GetAllStudents();
        public Students AddStudent(Students students);
    }
}
