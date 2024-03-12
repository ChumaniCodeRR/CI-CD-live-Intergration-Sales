using Integration_Sales_Order_Test.Model.Category;

namespace Integration_Sales_Order_Test.Repository.CategoryServices
{
    public interface ICategory
    {
        IEnumerable<CategoryResponse> GetAllCategories();

        CategoryResponse GetCategoryById(int categorycode);

        CategoryResponse CreateCategory(CategoryRequest model);

        CategoryResponse ModifyCategory(int categorycode, CategoryRequest model);

        void RemoveCategory(int categorycode, CategoryRequest model);
    }
}
