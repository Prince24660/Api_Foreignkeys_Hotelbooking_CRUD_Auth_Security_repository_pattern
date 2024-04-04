using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using AutoMapper;

namespace API_Foreignkey.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            //CreateMap<AdminForCreateVm, Admin>()
            //    .ForMember(a => a.Password,
            //    opt => opt.Ignore());

            //CreateMap<Admi, Admin>()
            //    .ForMember(a => a.Password,
            //    opt => opt.Ignore());

            CreateMap<Admin, AdminVM>();
        }
    }
}
