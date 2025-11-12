
using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

//add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddAutoMapper(cfg => { }, typeof(ProductMappingProfile).Assembly);


var app = builder.Build();

//configure http request pipeline
app.MapCarter();

app.Run();
