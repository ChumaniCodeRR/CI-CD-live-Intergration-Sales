using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
