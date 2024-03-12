using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Repository.ProductServices;
using Integration_Sales_Order_Test.Model.Products;
using Integration_Sales_Order_Test.Model.Accounts;

namespace Integration_Sales_Order_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        public ProductsController(IProductServices productServices, IMapper mapper)
        {
            _productServices = productServices;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductResponse>> GetAllProducts()
        {
            var products = _productServices.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            var product = _productServices.GetProductById(id);
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<ProductResponse> CreateProduct(ProductRequest model)
        {
            _productServices.CreateProduct(model);
            return Ok(new { message = "Product created successfully" });
        }

        [HttpPut("{productId:int}")]
        public ActionResult<ProductResponse> UpdateProduct(int productId, ProductRequest model)
        {

            if (productId != model.ProductId && model.Taxable != true)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            _productServices.ModifyProduct(productId, model);
            return Ok(new { message = "Product modified successfully" });
        }

        [HttpDelete("{productId:int}")]
        public IActionResult RemoveProduct(int productId, ProductRequest model)
        {
            if (productId != model.ProductId )
                return Unauthorized(new { message = "Unauthorized" });

            _productServices.RemoveProduct(productId, model);

            return Ok(new { message = "Product removed successfully" });
        }
    }
}
