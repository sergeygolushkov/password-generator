namespace API.Endpoints.Password;

public class CreatePasswordResponseV1
{
    public required string Password { get; set; }
    public required DateTime CreatedAt { get; set; }
}
