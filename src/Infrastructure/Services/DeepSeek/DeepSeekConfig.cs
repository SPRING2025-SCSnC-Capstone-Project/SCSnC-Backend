namespace Infrastructure.Services.Deepseek;

public class DeepSeekConfig
{
    public const string Section = "DeepSeek";
    
    public string ApiKey { get; set; }
    public string ApiUrl { get; set; }
    public string Model { get; set; }
}