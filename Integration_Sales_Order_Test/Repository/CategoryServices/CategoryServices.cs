using Integration_Sales_Order_Test.Model.Category;
using AutoMapper;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Repository.ProductServices;
using Integration_Sales_Order_Test.Model.Products;

namespace Integration_Sales_Order_Test.Repository.CategoryServices
{
    public class CategoryServices : ICategory
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public CategoryServices(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public CategoryResponse CreateCategory(CategoryRequest model)
        {
            if (_context.Category.Any(x => x.CategoryCode == model.CategoryCode))
                throw new AppException($"Category Name '{model.CategoryDesciption}' is already registered");

            var category = _mapper.Map<Category>(model);

            _context.Category.Add(category);
            _context.SaveChanges();

            return _mapper.Map<CategoryResponse>(category);

        }

        public CategoryResponse ModifyCategory(int categorycode, CategoryRequest model)
        {
            var category = getCategory(categorycode);

            if (category.CategoryCode != model.CategoryCode && _context.Category.Any(x => x.CategoryCode == model.CategoryCode))
                throw new AppException($"Category Name '{model.CategoryDesciption}' is already registered");

            _mapper.Map(model, category);
            _context.Category.Update(category);
            _context.SaveChanges();

            return _mapper.Map<CategoryResponse>(category);

        }

        public IEnumerable<CategoryResponse> GetAllCategories()
        {
            var category = _context.Category;
            return _mapper.Map<IList<CategoryResponse>>(category);
        }

        public CategoryResponse GetCategoryById(int categorycode)
        {
            var category = getCategory(categorycode);
            return _mapper.Map<CategoryResponse>(categorycode);
        }

        public void RemoveCategory(int categorycode, CategoryRequest model)
        {
            var category = getCategory(categorycode);
            _context.Category.Remove(category);
            _context.SaveChanges();
        }

        private Category getCategory(int categorycode)
        {
            var category = _context.Category.Find(categorycode);
            if (category == null) throw new KeyNotFoundException("Product not found");
            return category;
        }
    }
}
