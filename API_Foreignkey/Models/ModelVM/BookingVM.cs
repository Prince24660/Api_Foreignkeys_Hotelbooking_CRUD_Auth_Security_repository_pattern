namespace API_Foreignkey.Models.ModelVM
{
    public class BookingVM
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfPerson { get; set; }
        public int? ClientId { get; set; }
        public int? RoomId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
