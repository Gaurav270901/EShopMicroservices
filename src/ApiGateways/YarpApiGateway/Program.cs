using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//adding services to the container

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 3;
    });
});

var app = builder.Build();

//configure the http request pipelines
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
