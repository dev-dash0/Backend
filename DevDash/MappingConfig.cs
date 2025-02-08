using AutoMapper;
using DevDash.DTO;
using DevDash.DTO.Tenant;
using DevDash.DTO.User;
using DevDash.model;

namespace DevDash
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Tenant,TenantDTO>().ReverseMap();
            CreateMap<Tenant,TenantCreateDTO>().ReverseMap();
            CreateMap<Tenant,TenantUpdateDTO>().ReverseMap();
            CreateMap<TenantDTO, TenantCreateDTO>().ReverseMap();
            CreateMap<TenantDTO,TenantUpdateDTO>().ReverseMap();

            CreateMap<User,UserDTO>().ReverseMap();
        }
    }
}
