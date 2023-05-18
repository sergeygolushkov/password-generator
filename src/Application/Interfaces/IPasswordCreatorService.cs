using Application.Models;

namespace Application.Interfaces;

public interface IPasswordCreatorService
{
    string CreatePassword(CreatePasswordOptions options);
}
