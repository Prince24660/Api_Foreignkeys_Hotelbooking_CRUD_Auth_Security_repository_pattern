using System.ComponentModel.DataAnnotations;

namespace Test2.Models
{
    public class Skill
    {
        [Key]
        public int Sklid { get; set; }
        public string Sklname { get; set; }
    }
}
