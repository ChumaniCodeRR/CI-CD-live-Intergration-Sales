using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}