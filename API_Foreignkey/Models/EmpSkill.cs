using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class EmpSkill
    {
        [Key]
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int Sklid { get; set; }
       
    }
}
