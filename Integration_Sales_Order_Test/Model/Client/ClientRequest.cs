using Integration_Sales_Order_Test.Entities;
using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Client
{
    public class ClientRequest
    {
        [Key]
        public int ClientID { get; set; }
        [Required]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        public string ClientSurname { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string IndustryType { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string EmailAddress { get; set; }
       
    }
}
