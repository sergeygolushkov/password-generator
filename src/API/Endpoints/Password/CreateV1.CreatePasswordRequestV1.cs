namespace API.Endpoints.Password;

public class CreatePasswordRequestV1
{
    public bool IncludeUpperCaseLetters { get; set; }
    public bool IncludeNumbers { get; set; }
    public bool IncludeSpecialCharacters { get; set; }
    public short Length { get; set; }
}
