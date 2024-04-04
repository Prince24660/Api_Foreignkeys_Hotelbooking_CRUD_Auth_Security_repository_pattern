using API_Foreignkey.Models.ModelVM;
using API_Foreignkey.Models;
using AutoMapper;

namespace API_Foreignkey.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddressVM, Address>()
              .ForMember(a => a.Id, opt => opt.Ignore());
            CreateMap<Address, AddressVM>();
            CreateMap<Client, ClientVM>();
            CreateMap<ClientVM, Client>();
            CreateMap<ClientForCreateionVM, Client>();
            //CreateMap<ClientForUpdateDTO, Client>();
            //CreateMap<Client, ClientForUpdateDTO>();
        }
    }
}
