using AutoMapper;
using Extensions.ObjectMappers.Abstractions;

namespace ObjectMappersApp.Handler;

public class UserEntity
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserView
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
}

public record RoleEntity;
public record RoleDTO;
public record RoleView;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDTO>();

        CreateMap<UserDTO, UserView>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Email));
    }
}
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleEntity, RoleDTO>();
        CreateMap<RoleDTO, RoleView>();
    }
}
public class ServiceHandler
{
    private readonly IMapperAdapter _mapper;

    public ServiceHandler(IMapperAdapter mapperAdapter)
    {
        _mapper = mapperAdapter;
    }

    public UserDTO ConvertToDto(UserEntity entity)
        => _mapper.Map<UserEntity, UserDTO>(entity);

    public UserView ConvertToView(UserDTO dto)
        => _mapper.Map<UserDTO, UserView>(dto);

    public List<RoleView> ConvertRoleToViews(List<RoleEntity> roles)
        => _mapper.Map<RoleEntity, RoleView>(roles);
}
