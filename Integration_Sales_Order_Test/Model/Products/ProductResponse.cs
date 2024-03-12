using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Products
{
    public class ProductResponse
    {
        public int ProductID { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal WholeSalePrice { get; set; }
        
        public decimal ListPrice { get; set; }
        public int QtyStock { get; set; }
        public string ProductType { get; set; }
        
        public int Weight { get; set; }
        public bool Taxable { get; set; }
    }
}
