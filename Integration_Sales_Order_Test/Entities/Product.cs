using System;
using System.Collections.Generic;
using System.Data;

namespace Integration_Sales_Order_Test.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal WholeSalePrice { get; set; }

        public decimal ListPrice { get; set; }

        public int QtyStock { get; set; }   

        public string ProductType { get; set; }

        public string CategoryCode {get; set; } 

        public int Weight { get; set; } 

        public bool Taxable { get; set; }

        public DateTime DateAvailable { get; set; }

    }
}
