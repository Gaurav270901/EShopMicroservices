
//things to do 
//learn about proxy pattern and decorator pattern
//scrutor library


using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

//add servces to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter(); 
builder.Services.AddMediatR(config =>
{
       config.RegisterServicesFromAssembly(assembly);
       config.AddOpenBehavior(typeof(ValidationBehavior<,>));
       config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository , BasketRepository>();

//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepo = provider.GetRequiredService<IBasketRepository>();
//    return new CachedBasketRepository(basketRepo, provider.GetRequiredService(IDistributedCache cache));
//});
//scrutor helps to decorate method more efficiently
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
var app = builder.Build();

// configure the http request pipeline
app.UseExceptionHandler(option => { });
app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.MapCarter();
app.Run();
