using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Model.Orders;
using Integration_Sales_Order_Test.Repository.Orders;
using Integration_Sales_Order_Test.Repository.CategoryServices;
using Integration_Sales_Order_Test.Model.Category;

namespace Integration_Sales_Order_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IOrders _order;
        private readonly IMapper _mapper;


        public OrdersController(IOrders order, IMapper mapper)
        {
            _order = order;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<OrderResponse>> GetAllOrders()
        {
            var orders = _order.GetAllOrders();
            return Ok(orders);
        }


        [HttpGet("{orderNum:int}")]
        public ActionResult<OrderResponse> GetOrderById(int orderNum)
        {
          
            var order = _order.GetOrderById(orderNum);
            
            return Ok(order);
        }


        [HttpPost]
        public ActionResult<OrderResponse> CreateOrder(OrderRequest model)
        {
            _order.CreateOrder(model);
            return Ok(new { message = "Order created successfully" });
        }

        [HttpPut("{orderNum:int}")]
        public ActionResult<OrderResponse> ModifyOrder(int orderNum, OrderRequest model)
        {
            if (orderNum != model.OrderNum)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            _order.ModifyOrder(orderNum, model);

            return Ok(new { message = "Order modified successfully" });
        }


        [HttpDelete("{orderNum:int}")]
        public IActionResult RemoveOrder(int orderNum, OrderRequest model)
        {
            if (orderNum != model.OrderNum)
                return Unauthorized(new { message = "Unauthorized" });

            _order.RemoveOrder(orderNum, model);

            return Ok(new { message = "Order removed successfully" });
        }
    }
}
