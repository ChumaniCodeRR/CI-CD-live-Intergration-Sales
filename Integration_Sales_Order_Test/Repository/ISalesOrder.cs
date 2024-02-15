using Integration_Sales_Order_Test.Model; 

namespace Integration_Sales_Order_Test.Repository
{
    public interface ISalesOrder
    {
      
        Task<string> UploadAsync(string path, IFormFile formFile);
       
    }
}
