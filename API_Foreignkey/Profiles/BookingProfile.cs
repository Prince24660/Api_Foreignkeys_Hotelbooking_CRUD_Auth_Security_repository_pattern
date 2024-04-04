using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using AutoMapper;

namespace API_Foreignkey.Profiles
{
    public class BookingProfile :Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingVM, Booking>();
            CreateMap<Booking, BookingVM>();
            CreateMap<BookingForCreateionVM, Booking>()
                .ForMember(b => b.CheckInDate,
                opts => opts.MapFrom(src => src.BookingDates.CheckInDate))
                .ForMember(b => b.CheckOutDate,
                opts => opts.MapFrom(src => src.BookingDates.CheckOutDate));

            //CreateMap<BookingForUpdateDTO, Booking>()
            //   .ForMember(b => b.CheckInDate,
            //   opts => opts.MapFrom(src => src.BookingDates.CheckInDate))
            //   .ForMember(b => b.CheckOutDate,
            //   opts => opts.MapFrom(src => src.BookingDates.CheckOutDate));

        }
    }
}
