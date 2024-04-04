using API_Foreignkey.Models.ModelVM;
using API_Foreignkey.Models;
using API_Foreignkey.ResourceParameter;

namespace API_Foreignkey.IReposiory
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsAsync(BookingReSourceParameters bookingsResourceParameters);
        Task<Booking> GetBookingAsync(int id);
        Task<bool> IsBookingExistsAsync(int id);
        Task<bool> IsRoomVacancyAsync(int roomId, DatesVM dates, int? bookingId = null);
        Task<bool> IsRoomExistsAsync(int roomId);
        Task<bool> EditBookingDatesAsync(int bookingId, DatesVM newDates);
        Task<decimal> CalculateTotalPrice(int roomId, int numberOfPerson, DatesVM dates);
        bool AreDatesCorrect(DatesVM newDates);
        void UpdateBooking(Booking booking);
    }
}
