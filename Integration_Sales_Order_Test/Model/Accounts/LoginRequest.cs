using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Accounts
{
    public class LoginRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
