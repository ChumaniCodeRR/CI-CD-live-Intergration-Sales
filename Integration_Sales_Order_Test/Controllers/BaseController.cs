using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Integration_Sales_Order_Test.Entities;

namespace Integration_Sales_Order_Test.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public Account Account => (Account)HttpContext.Items["Account"];

        public Product Product => (Product)HttpContext.Items["Product"];

        public Category Category => (Category)HttpContext.Items["Category"];

        public Client Client => (Client)HttpContext.Items["Client"];

        public ItemOrders ItemOrders => (ItemOrders)HttpContext.Items["ItemOrders"];

    }
}
