using Integration_Sales_Order_Test.Repository;
using Integration_Sales_Order_Test.Model;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Integration_Sales_Order_Test.Repository
{
    public class SalesOrderRepo : ISalesOrder
    {

        public SalesOrderRepo(/*DataAppContext ctx*/)
        {
            // _ctx = ctx;
            // var salesOrder = await _ctx.Currency_Info.ToListAsync();
            // return salesOrder;
        }

        public async Task<string> UploadAsync(string path, IFormFile file)
        {
            try
            {
                CancellationToken cancellationToken = default;

                if (file.Length < 1) throw new Exception($"No file found!");

                var allowedFileTypes = new[] { "png", "jpg", "json" };

                // Get the file extension.
                var fileExtension = Path.GetExtension(file.FileName).Substring(1);

                // validate the file extension type.
                if (!allowedFileTypes.Contains(fileExtension))
                {
                    throw new Exception($"File format {Path.GetExtension(file.FileName)} is invalid for this operation.");
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                await using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;

                // Write file to System path
                var filePath = Path.Combine("wwwroot", "Files", fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);

                await memoryStream.CopyToAsync(fileStream, cancellationToken);

                return $"{path}/api/Files/{fileName}";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
