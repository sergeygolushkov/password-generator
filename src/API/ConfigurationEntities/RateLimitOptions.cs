namespace API.ConfigurationEntities;

public class RateLimitOptions
{
    public const string ConfigurationSectionName = "RateLimit";

    public int PermitLimit { get; set; } = 10;
    public int QueueLimit { get; set; } = 5;
    public int WindowSeconds { get; set; } = 1;

}
