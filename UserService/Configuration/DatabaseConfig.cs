namespace UserService.Configuration;

public record DatabaseConfig
{
    public string DatabaseSecureConnectPath { get; set; }
    public string DefaultKeyspace { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
