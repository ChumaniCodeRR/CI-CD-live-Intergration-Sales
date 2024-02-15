using Integration_Sales_Order_Test.Model;

namespace Integration_Sales_Order_Test.Model
{
    public class SalesOrder
    {
        public string salesOrderRef {  get; set; }

        public DateTime? orderDate { get; set; }

        public string currencyCode { get; set; }

        public DateTime? shipDate { get; set; }

        public string categoryCode { get; set; }    

        public Addresses addresses { get; set; }

        public List<OrderLines> orderLines { get; set; }

      //  public string orderLines { get; set; }


    }
}
