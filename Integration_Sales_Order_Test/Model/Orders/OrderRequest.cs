using Integration_Sales_Order_Test.Entities;
using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Orders
{
    public class OrderRequest
    {
        [Required]
        public int OrderNum { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
    }
}
