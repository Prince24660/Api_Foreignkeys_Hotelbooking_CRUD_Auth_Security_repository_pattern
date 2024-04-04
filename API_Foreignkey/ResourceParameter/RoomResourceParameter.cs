using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;

namespace API_Foreignkey.ResourceParameter
{
    public class RoomResourceParameter : RessourceParameteres
    {
        public bool? HasBalcony { get; set; }
        public RoomType? RoomType { get; set; }
        public decimal? PriceLessThan { get; set; }
        public int? NumberOfPerson { get; set; }
        public DatesVM VacancyInDays { get; set; }
    }
}
