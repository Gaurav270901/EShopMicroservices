namespace Catalog.API.Exceptions
{
    public class ProductWithCategoryNotFoundException : Exception
    {
        public ProductWithCategoryNotFoundException(string category) :
            base("Product with category not fount")
        { }
    }
}
