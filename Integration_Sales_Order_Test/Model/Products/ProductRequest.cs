using Integration_Sales_Order_Test.Entities;
using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Products
{
    public class ProductRequest
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal WholeSalePrice { get; set; }
        [Required]
        public decimal ListPrice { get; set; }
        [Required]
        public int QtyStock { get; set; }
        [Required]
        public string ProductType { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public bool Taxable { get; set; }
    }
}
