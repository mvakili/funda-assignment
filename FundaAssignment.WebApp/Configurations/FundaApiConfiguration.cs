namespace FundaAssignment.WebApp.Configurations;
public class FundaApiConfiguration
{
    public string BaseUrl { get; set; }
    public string ApiKey { get; set; }
    public int ThroughputPerMinute { get; set; }
    public int RetryOnFailCount { get; set; }
}
