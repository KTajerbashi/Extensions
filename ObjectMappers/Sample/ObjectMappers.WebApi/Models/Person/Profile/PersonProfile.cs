using AutoMapper;

namespace ObjectMappers.WebApi.Models.Person;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PersonEntity, PersonCommand>().ReverseMap();
        CreateMap<PersonEntity, PersonQuery>().ReverseMap();
        CreateMap<PersonEntity, PersonView>().ReverseMap();
        CreateMap<PersonEntity, PersonDto>().ReverseMap();
    }
}
