using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using API_Foreignkey.ResourceParameter;
using Microsoft.EntityFrameworkCore;
using Test2.Data;

namespace API_Foreignkey.IReposiory.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool AreDatesCorrect(DatesVM newDates)
        {
           if(newDates == null)
            
                return false;

                return newDates.CheckInDate.Date> DateTime.Now && newDates.CheckOutDate.Date > DateTime.Now;
            
        }

        public async Task<decimal> CalculateTotalPrice(int roomId, int numberOfPerson, DatesVM dates)
        {
            var room = await _context.rooms.FindAsync(roomId);

            if (room == null)
                return 0;

            //full 24h for example. 12.11 - 14.11 gives 48h (booking from 11 o'clock 12.11 to 11 o'clock 14.11)
            int days = (dates.CheckOutDate.Date - dates.CheckInDate.Date).Days;

            return (decimal)(room.PriceForDay * numberOfPerson * days);

        }

        public async Task<bool> EditBookingDatesAsync(int bookingId, DatesVM newDates)
        {
            try
            {
                var booking=await _context.bookings.FindAsync(bookingId);
                booking.CheckInDate = newDates.CheckInDate;
                booking.CheckOutDate = newDates.CheckOutDate;
                _context.Update(booking);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Booking> GetBookingAsync(int id)
        {
            return await _context.bookings.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            return await _context.bookings.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync(BookingReSourceParameters bookingsResourceParameters)
        {
           if(bookingsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(bookingsResourceParameters));
            }
           if(bookingsResourceParameters.RoomId==null
                && bookingsResourceParameters.ClientId==null
                && bookingsResourceParameters.CurrentBookings == null)
            {
                return await GetBookingsAsync();
            }
            var collection = _context.bookings as IQueryable<Booking>;
            if (!(bookingsResourceParameters.RoomId == null))
            {
                collection = collection.Where(b => b.RoomId == bookingsResourceParameters.RoomId);
            }

            if (!(bookingsResourceParameters.ClientId == null))
            {
                collection = collection.Where(b => b.ClientId == bookingsResourceParameters.ClientId);
            }

            if (!(bookingsResourceParameters.CurrentBookings == null || bookingsResourceParameters.CurrentBookings == false))
            {
                collection = collection.Where(b => b.CheckOutDate.Date >= DateTime.Now.Date && b.CheckInDate.Date <= DateTime.Now.Date);
            }

            return await collection.ToListAsync();
        }

        public async Task<bool> IsBookingExistsAsync(int id)
        {
           return await _context.bookings.AnyAsync(a=>a.Id == id);  
        }

        public async Task<bool> IsRoomExistsAsync(int roomId)
        {
            return await _context.rooms.AnyAsync(a=>a.Id == roomId);
        }


        public async Task<bool> IsRoomVacancyAsync(int roomId, DatesVM dates, int? bookingId = null)
        {
            DateTime currentDate = DateTime.Today;
            bool isRoomVacancy = false;

            if (currentDate < dates.CheckOutDate.Date && currentDate < dates.CheckInDate.Date)
            {
                // Check if there are no bookings for the given room for the specified date range
                if (!await _context.bookings.AnyAsync(b => b.RoomId == roomId && b.Id != bookingId &&
                        !(b.CheckOutDate.Date <= dates.CheckInDate.Date || b.CheckInDate.Date >= dates.CheckOutDate.Date)))
                {
                    return true; // Room is vacant for the specified date range
                }
                else
                {
                    // Check if booking for the same date range is attempted, if yes return false
                    if (await _context.bookings.AnyAsync(b => b.RoomId == roomId && b.Id != bookingId &&
                        b.CheckInDate.Date == dates.CheckInDate.Date && b.CheckOutDate.Date == dates.CheckOutDate.Date))
                    {
                        return false; // Room is already booked for the specified date range
                    }
                    else
                    {
                        return true; // Room is available for a different date range
                    }
                }
            }
            else
            {
                // If the specified dates are not valid (e.g., checkout date is before today or check-in date), room is not available
                isRoomVacancy = false;
            }

            return isRoomVacancy;
        }

        public void UpdateBooking(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
