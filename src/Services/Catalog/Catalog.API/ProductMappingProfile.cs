
using Catalog.API.Products.GetProduct;
using Catalog.API.Products.GetProductById;
using Catalog.API.Products.GetProducts;

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

            CreateMap<GetProductResult, GetProductResponse>().ReverseMap();
            CreateMap<GetProductByIdResonse , GetProductByIdResult>().ReverseMap();
        }
    }
}