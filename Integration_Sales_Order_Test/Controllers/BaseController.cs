using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Integration_Sales_Order_Test.Entities;

namespace Integration_Sales_Order_Test.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        public Account Account => (Account)HttpContext.Items["Account"];

    }
}
