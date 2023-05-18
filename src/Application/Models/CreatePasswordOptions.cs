namespace Application.Models;

public class CreatePasswordOptions
{
    public bool IncludeUpperCaseLetters { get; set; } = true;
    public bool IncludeNumbers { get; set; } = true;
    public bool IncludeSpecialCharacters { get; set; } = true;
    public short Length { get; set; } = 16;
}
