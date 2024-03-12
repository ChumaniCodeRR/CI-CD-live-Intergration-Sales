using Integration_Sales_Order_Test.Model.Orders;

namespace Integration_Sales_Order_Test.Repository.Orders
{
    public interface IOrders
    {
        IEnumerable<OrderResponse> GetAllOrders();

        OrderResponse GetOrderById(int id);

        OrderResponse CreateOrder(OrderRequest model);

        OrderResponse ModifyOrder(int id, OrderRequest model);

        void RemoveOrder(int id, OrderRequest model);
    }
}
