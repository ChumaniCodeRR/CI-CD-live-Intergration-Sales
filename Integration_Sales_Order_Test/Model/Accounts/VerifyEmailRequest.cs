using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
