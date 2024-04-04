using Test2.Models;

namespace API_Foreignkey.Models
{
    public class EmployeeWithSkills
    {
        public Employee Employee { get; set; }
        public List<int> Sklids { get; set; }
    }
}
