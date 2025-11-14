
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.GetProduct;
using Catalog.API.Products.GetProductByCategory;
using Catalog.API.Products.GetProductById;
using Catalog.API.Products.GetProducts;
using Catalog.API.Products.UpdateProduct;
using Marten.Linq.CreatedAt;

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
            CreateMap<GetProductByCategoryResponse, GetProductByCategoryResult>().ReverseMap();
            CreateMap<UpdateProductCommand, UpdateProductRequest>().ReverseMap();
            CreateMap<UpdateProductResponse, UpdateProductResult>().ReverseMap();
            CreateMap<DeleteProductResponse, DeleteProductResult>().ReverseMap();
        }
    }
}