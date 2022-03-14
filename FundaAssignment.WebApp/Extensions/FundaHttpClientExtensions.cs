using Polly;
using ComposableAsync;
using RateLimiter;
using System.Net;
using FundaAssignment.WebApp.Configurations;

namespace FundaAssignment.WebApp.Extensions;

public static class FundaHttpClientExtensions
{
    /// <summary>
    /// Creates an http client for Funda API. 
    /// This http client handles rate limit (<= 100req/min)
    /// Also retries requests in case of rate limit violation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="fundaApiSettings"></param>
    /// <returns></returns>
    public static IServiceCollection AddFundaHttpClient(this IServiceCollection services, FundaApiConfiguration fundaApiSettings)
    {
        var retryOnUnauthorizedPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
            .RetryAsync(retryCount: fundaApiSettings.RetryOnFailCount);

        services
            .AddHttpClient("funda-api", config =>
                 {

                     config.BaseAddress = new Uri($"{fundaApiSettings.BaseUrl}/{fundaApiSettings.ApiKey}");
                 })
            .AddPolicyHandler(retryOnUnauthorizedPolicy)
            .AddHttpMessageHandler(() =>
                new RateLimitHttpMessageHandler(
                    limitCount: fundaApiSettings.ThroughputPerMinute,
                    limitTime: TimeSpan.FromMinutes(1)))
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        return services;
    }
}
internal class RateLimitHttpMessageHandler : DelegatingHandler
{
    private readonly TimeLimiter? timeConstraint;
    public RateLimitHttpMessageHandler(int limitCount, TimeSpan limitTime)
    {
        timeConstraint = TimeLimiter.GetFromMaxCountByInterval(limitCount, limitTime);
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await timeConstraint;
        return await base.SendAsync(request, cancellationToken);
    }
}
