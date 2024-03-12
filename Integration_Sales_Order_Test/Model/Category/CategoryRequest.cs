using Integration_Sales_Order_Test.Entities;
using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Category
{
    public class CategoryRequest
    {
        [Key]
        [Required]
        public int CategoryCode { get; set; } 

        [Required]
        public string CategoryDesciption { get; set; }

    }
}
