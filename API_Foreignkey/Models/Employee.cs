using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test2.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public int DepId { get; set; }
        public int Sklid { get; set; }
    }
        
    public class ServiceResponse
    {
        public string message { get; set; }
        public object data { get; set; }
        public bool issuccess { get; set; }
        public string logintype { get; set; }
    }
}
