namespace API_Foreignkey.Models.ModelVM
{
    public class RoomForManuplationVM
    {
        public RoomType? Type { get; set; }
        public bool HasBalcony { get; set; }
        public string Description { get; set; }
        public decimal PriceForDay { get; set; }
        public int MaxNumberOfPerson { get; set; }
    }
}
