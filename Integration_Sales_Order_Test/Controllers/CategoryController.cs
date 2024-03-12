using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Model.Category;
using Integration_Sales_Order_Test.Repository.CategoryServices;

namespace Integration_Sales_Order_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategory _category;
        private readonly IMapper _mapper;

        public CategoryController(ICategory category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public ActionResult<IEnumerable<CategoryResponse>> GetAllCategory()
        {
            var categories = _category.GetAllCategories();
            return Ok(categories);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{categorycode:int}")]
        public ActionResult<CategoryResponse> GetCategoryById(int categorycode)
        {
            var product = _category.GetCategoryById(categorycode);
            return Ok(product);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public ActionResult<CategoryResponse> CreateCategory(CategoryRequest model)
        {
            _category.CreateCategory(model);
            return Ok(new { message = "Category created successfully" });
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{categorycode:int}")]
        public ActionResult<CategoryResponse> ModifyCategory(int categorycode , CategoryRequest model)
        {
            if (categorycode != model.CategoryCode)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            _category.ModifyCategory(categorycode, model);
            
            return Ok(new { message = "Category modified successfully" });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{categorycode:int}")]
        public IActionResult RemoveCategory(int categorycode, CategoryRequest model)
        {
            if (categorycode != model.CategoryCode)
                return Unauthorized(new { message = "Unauthorized" });

            _category.RemoveCategory(categorycode,model);
            return Ok(new { message = "Category removed successfully" });
        }
    }
}
