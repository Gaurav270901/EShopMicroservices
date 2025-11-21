

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
var app = builder.Build();

// configure the http request pipeline

app.MapCarter();
app.Run();
