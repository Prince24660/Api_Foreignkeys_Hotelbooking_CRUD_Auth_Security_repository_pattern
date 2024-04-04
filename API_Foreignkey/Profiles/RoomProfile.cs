using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using AutoMapper;

namespace API_Foreignkey.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomVM, Room>();
            CreateMap<RoomForCreateionVM, Room>();
            CreateMap<Room, RoomVM>();
            //CreateMap<RoomForUpdateDTO, Room>();
            //CreateMap<Room, RoomForUpdateDTO>();

        }
    }
}
