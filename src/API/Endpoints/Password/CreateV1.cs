using Application.Interfaces;
using Application.Models;
using FastEndpoints;

namespace API.Endpoints.Password;

public class CreateV1 : Endpoint<CreatePasswordRequestV1, CreatePasswordResponseV1>
{
    private readonly IPasswordCreatorService _passwordCreatorService;
    private readonly AutoMapper.IMapper _mapper;

    public CreateV1(IPasswordCreatorService passwordCreatorService, AutoMapper.IMapper mapper)
    {
        _passwordCreatorService = passwordCreatorService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("password/create");
        Version(1);
        AllowAnonymous();

        Description(b => b
            .WithName("Create Password")
            .Accepts<CreatePasswordRequestV1>("application/json")
            .Produces<CreatePasswordResponseV1>(200, "application/json")
            .ProducesProblemFE(400)
            .ProducesProblemFE<InternalErrorResponse>(500));
        Summary(s =>
        {
            s.ExampleRequest = new CreatePasswordRequestV1
            {
                IncludeNumbers = true,
                IncludeSpecialCharacters = true,
                IncludeUpperCaseLetters = true,
                Length = 16
            };
            s.ResponseExamples[200] = new CreatePasswordResponseV1
            {
                CreatedAt = DateTime.UtcNow,
                Password = "zxvo9608AHCW+(_>"
            };
        });
    }

    public override async Task HandleAsync(CreatePasswordRequestV1 req, CancellationToken ct)
    {
        var password = _passwordCreatorService.CreatePassword(_mapper.Map<CreatePasswordOptions>(req));
        CreatePasswordResponseV1 response = new() { CreatedAt = DateTime.UtcNow, Password = password };
        await SendAsync(response);
    }

}
