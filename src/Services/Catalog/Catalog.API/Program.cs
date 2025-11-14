
using BuildingBlocks.Behavior;
using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

//add services to container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    
 });

builder.Services.AddAutoMapper(cfg => { }, typeof(ProductMappingProfile).Assembly);

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions(); 

var app = builder.Build();

//configure http request pipeline
app.MapCarter();

app.Run();
