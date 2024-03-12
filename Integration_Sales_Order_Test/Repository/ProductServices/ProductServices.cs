using AutoMapper;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Model.Products;
using Integration_Sales_Order_Test.Model.Accounts;
using Integration_Sales_Order_Test.Repository.ServicesEmail;

namespace Integration_Sales_Order_Test.Repository.ProductServices
{
    public class ProductServices : IProductServices
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public ProductServices(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }


        public ProductResponse CreateProduct(ProductRequest model)
        {
            if (_context.Products.Any(x => x.Name == model.Name))
                throw new AppException($"Product Name '{model.Name}' is already registered");

            var product = _mapper.Map<Product>(model);

            if (product.Price == 0)
                throw new AppException("Price cannot be zero.");

            if (product.WholeSalePrice == 0)
                throw new AppException("Whole Sale Price cannot be zero.");

            if (product.ListPrice == 0)
                throw new AppException("List Price cannot be zero.");

            product.DateAvailable = DateTime.UtcNow;
            product.Taxable = true;

            _context.Products.Add(product);
            _context.SaveChanges();

            return _mapper.Map<ProductResponse>(product);

        }

        public ProductResponse ModifyProduct(int id, ProductRequest model)
        {
            var product = getProduct(id);

            if (product.Name != model.Name && _context.Products.Any(x => x.Name == model.Name))
                throw new AppException($"Product name '{model.Name}' is already taken");

            if (product.Price == 0)
                throw new AppException("Price cannot be zero.");

            if (product.WholeSalePrice == 0)
                throw new AppException("Whole Sale Price cannot be zero.");

            if (product.ListPrice == 0)
                throw new AppException("List Price cannot be zero.");

            _mapper.Map(model, product);
            _context.Products.Update(product);
            _context.SaveChanges();

            return _mapper.Map<ProductResponse>(product);

        }
        public IEnumerable<ProductResponse> GetAllProducts()
        {
            var products = _context.Products;
            return _mapper.Map<IList<ProductResponse>>(products);
        }

        public ProductResponse GetProductById(int id)
        {
            var product = getProduct(id);
            return _mapper.Map<ProductResponse>(product);
        }

        public void RemoveProduct(int id, ProductRequest model)
        {
            var product = getProduct(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        private Product getProduct(int id)
        {
            var account = _context.Products.Find(id);
            if (account == null) throw new KeyNotFoundException("Product not found");
            return account;
        }
    }
}
