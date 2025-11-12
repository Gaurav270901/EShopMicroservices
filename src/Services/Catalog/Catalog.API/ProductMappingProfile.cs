
namespace Catalog.API.Products.CreateProduct
{
    internal class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductRequest, CreateProductCommand>()
                .ConstructUsing(src => new CreateProductCommand(
                    src.Name,
                    src.Category ?? new List<string>(),
                    src.Description,
                    src.ImageFile,
                    src.Price));

           CreateMap<CreateProductResult, CreateProductResponse>()
                .ConstructUsing(src => new CreateProductResponse(src.Id));
        }
    }
}