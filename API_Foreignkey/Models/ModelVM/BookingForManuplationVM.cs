namespace API_Foreignkey.Models.ModelVM
{
    public class BookingForManuplationVM
    {
        public DatesVM BookingDates { get; set; }
        public int NumberOfPerson { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
    }
}
