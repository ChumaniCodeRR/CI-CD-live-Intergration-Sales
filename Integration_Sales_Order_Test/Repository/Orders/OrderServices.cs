using AutoMapper;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Model.Orders;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Model.Client;

namespace Integration_Sales_Order_Test.Repository.Orders
{
    public class OrderServices : IOrders
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public OrderServices(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public OrderResponse GetOrderById(int orderNum)
        {
            var order = getOrder(orderNum);
            return _mapper.Map<OrderResponse>(order);
        }

        public IEnumerable<OrderResponse> GetAllOrders()
        {
            var orders = _context.ItemsOrders;
            return _mapper.Map<IList<OrderResponse>>(orders);
        }

        public OrderResponse CreateOrder(OrderRequest model)
         {
             if (_context.ItemsOrders.Any(x => x.OrderNum == model.OrderNum))
                 throw new AppException($"Order Number '{model.OrderNum }' is already registered");

             var order = _mapper.Map<ItemOrders>(model);
              
            order.OrderStatus = "Pending";
            order.OrderDate = DateTime.Now;
           
             _context.ItemsOrders.Add(order);
             _context.SaveChanges();

             return _mapper.Map<OrderResponse>(order);
         }

        public OrderResponse ModifyOrder(int id, OrderRequest model)
        {
            var order = getOrder(id);

            if (order.OrderNum != model.OrderNum && _context.ItemsOrders.Any(x => x.OrderNum == model.OrderNum))
                throw new AppException($"Order Number ' { model.OrderNum } ' is already taken");

            _mapper.Map(model, order);
            _context.ItemsOrders.Update(order);
            _context.SaveChanges();

            return _mapper.Map<OrderResponse>(order);

        }

        public void RemoveOrder(int id, OrderRequest model)
        {
            var client = getOrder(id);
            _context.ItemsOrders.Remove(client);
            _context.SaveChanges();
        }

        private ItemOrders getOrder(int id)
        {
            var order = _context.ItemsOrders.Find(id);
            if (order == null) throw new KeyNotFoundException("Order not found");
            return order;
        }
    }
}
