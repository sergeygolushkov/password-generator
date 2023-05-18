using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Password;

public class CreatePasswordRequestV1Validator : Validator<CreatePasswordRequestV1>
{
    public CreatePasswordRequestV1Validator()
    {
        RuleFor(x => x.Length)
            .NotEmpty()
            .WithMessage("Password length is required!")
            .GreaterThanOrEqualTo((short)8)
            .WithMessage("Password length should be greater than or equals to 8")
            .LessThanOrEqualTo((short)64)
            .WithMessage("Password length should be less than or equals to 64");

        RuleFor(x => x.IncludeNumbers)
            .NotEmpty()
            .WithMessage("Include numbers true/false is required!");

        RuleFor(x => x.IncludeSpecialCharacters)
            .NotEmpty()
            .WithMessage("Include special characters true/false is required!");

        RuleFor(x => x.IncludeUpperCaseLetters)
            .NotEmpty()
            .WithMessage("Include upper case letters true/false is required!");
    }
}
