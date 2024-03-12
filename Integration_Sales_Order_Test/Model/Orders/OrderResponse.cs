using System.ComponentModel;

namespace Integration_Sales_Order_Test.Model.Orders
{
    public class OrderResponse
    {
        public int OrderNum { get; set; }

        public DateTime OrderDate { get; set; } 

        public int AccountId { get; set; }  

        public string OrderStatus { get; set; }

        public DateTime RequiredDate { get; set; }
    }
}
