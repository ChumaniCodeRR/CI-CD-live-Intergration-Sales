using AutoMapper;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Model;
using Integration_Sales_Order_Test.Model.Accounts;
using Integration_Sales_Order_Test.Model.Category;
using Integration_Sales_Order_Test.Model.Client;
using Integration_Sales_Order_Test.Model.Orders;
using Integration_Sales_Order_Test.Model.Products;

namespace Integration_Sales_Order_Test.Helpers
{

    public class AutoMapperProfile : Profile
    {
        // mappings between model and entity objects
        public AutoMapperProfile()
        {
            //currency
           // CreateMap<Currency, Currency>();
            //order 
            CreateMap<OrderRequest, ItemOrders>();
            CreateMap<ItemOrders, OrderResponse>();
            //products
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
            //category
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            //client
            CreateMap<ClientRequest, Client>();
            CreateMap<Client, ClientResponse>();

            //login, register, authenctication account
            CreateMap<LoginRequest, UserLoginDetails>();
            CreateMap<Account, AccountResponse>();
            CreateMap<Account, AuthenticateResponse>();
            CreateMap<RegisterRequest, Account>();
            CreateMap<CreateRequest, Account>();
            CreateMap<UpdateRequest, Account>()

                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                        return true;
                    }
                ));
        }
    }
}

