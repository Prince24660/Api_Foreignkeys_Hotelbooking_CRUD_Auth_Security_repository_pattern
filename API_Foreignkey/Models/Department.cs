using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class Department
    {
        [Key]
        public int DepId { get; set; }
        public string DepName { get; set; }
    }
}
