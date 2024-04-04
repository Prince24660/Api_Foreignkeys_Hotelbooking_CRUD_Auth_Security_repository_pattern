using API_Foreignkey.IReposiory;
using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Foreignkey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICientRepository _cientRepository;
        private readonly IMapper _mapper;
        private readonly IDBRepository _dbRepository;
        public BookingController(ICientRepository cientRepository, IBookingRepository bookingRepository, IMapper mapper, IDBRepository dBRepository)
        {

            _cientRepository = cientRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _dbRepository = dBRepository;
        }
        [HttpPost]
        public async Task<ActionResult<BookingVM>> NewBooking(BookingForCreateionVM bookingForCreateionVM)
        {
          
            try
            {
                if (!await _bookingRepository.IsRoomVacancyAsync(bookingForCreateionVM.RoomId, bookingForCreateionVM.BookingDates))
                {
                    return Conflict();
                }
                Booking newbooking = _mapper.Map<Booking>(bookingForCreateionVM);
                newbooking.TotalPrice = await _bookingRepository.CalculateTotalPrice(bookingForCreateionVM.RoomId, (int)bookingForCreateionVM.NumberOfPerson, bookingForCreateionVM.BookingDates);
                _dbRepository.Add(newbooking);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetBooking), new { bookingId = newbooking.Id }, _mapper.Map<BookingVM>(newbooking));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return NotFound();
        }
        //[HttpGet]
        //public async Task<ActionResult<BookingVM>> GetAllBookings()
        //{
        //    var booking = _bookingRepository.GetBookingsAsync();
        //    return Ok(booking);
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingVM>>> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetBookingsAsync();
            return Ok(bookings);    
        }
        [HttpGet("{bookingId}")]
        public async Task<ActionResult<BookingVM>> GetBooking(int bookingId)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingAsync(bookingId);
                if (booking != null)
                {
                    return Ok(_mapper.Map<BookingVM>(booking));
                }
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return NotFound();
        }
       [HttpDelete("{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                var bookingToRemove = await _bookingRepository.GetBookingAsync(bookingId);
                if (bookingToRemove == null)
                {
                    return NotFound();
                
                }
               _dbRepository.Remove(bookingToRemove);
                if (await _dbRepository.SaveChangesAsync())
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Satabase Failure");
            }
            return BadRequest();
        }
    }
}
