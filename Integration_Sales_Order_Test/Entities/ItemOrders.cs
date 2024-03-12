using System;
using System.Collections.Generic;
using System.Data;

namespace Integration_Sales_Order_Test.Entities
{
    public class ItemOrders
    {
        public int OrderNum { get; set; }

        public DateTime OrderDate { get; set; }

        public int AccountId { get; set; }

        public string OrderStatus { get; set; }

        public DateTime RequiredDate { get; set; }
    }
}
