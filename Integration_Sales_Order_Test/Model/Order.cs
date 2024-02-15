using Newtonsoft.Json;

namespace Integration_Sales_Order_Test.Model
{
    public class Order
    {
        public string orderRef { get; set; }

        public DateTime orderDate { get; set; }

        public string currency {  get; set; }

        public DateTime shipDate { get; set; }

        public string categoryCode { get; set; }

        //public List<string> Orderinfo { get; set; }

    }
}
