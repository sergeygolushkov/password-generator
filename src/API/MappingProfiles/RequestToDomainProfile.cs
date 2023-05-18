using API.Endpoints.Password;
using Application.Models;
using AutoMapper;

namespace API.MappingProfiles;

public class RequestToDomainProfile : Profile
{
    public RequestToDomainProfile()
    {
        CreateMap<CreatePasswordRequestV1, CreatePasswordOptions>();
    }
}
