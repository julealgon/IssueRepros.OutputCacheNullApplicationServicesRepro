using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<CachingOptions>()
    .Configure(o => o.MyCustomExpiration = TimeSpan.FromMinutes(5))
    .Services
    .AddOutputCache(o =>
    {
        var myCustomOptions = o
            .ApplicationServices // <- this will be `null`
            .GetRequiredService<IOptions<CachingOptions>>()
            .Value;

        o.AddBasePolicy(b => b.Expire(myCustomOptions.MyCustomExpiration));
    });

var app = builder.Build();

app.MapGet("/ping", () => "ok");
app.UseOutputCache();

app.Run();

public class CachingOptions
{
    public TimeSpan MyCustomExpiration { get; set; }
}