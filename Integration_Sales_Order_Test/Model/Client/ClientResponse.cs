using System.ComponentModel.DataAnnotations;

namespace Integration_Sales_Order_Test.Model.Client
{
    public class ClientResponse
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public string ClientSurname { get; set; }

        public string Address { get; set; }

        public string IndustryType { get; set; }

        public string City { get; set; }

        public string EmailAddress { get; set; }
    }
}
