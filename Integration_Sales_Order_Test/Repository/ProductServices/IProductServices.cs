using Integration_Sales_Order_Test.Model.Accounts;
using Integration_Sales_Order_Test.Model.Products;
namespace Integration_Sales_Order_Test.Repository.ProductServices
{
    public interface IProductServices
    {
        IEnumerable<ProductResponse> GetAllProducts();

        ProductResponse GetProductById(int id);

        ProductResponse CreateProduct(ProductRequest model);

        ProductResponse ModifyProduct(int id, ProductRequest model);

        void RemoveProduct(int id, ProductRequest model);
    }
}
