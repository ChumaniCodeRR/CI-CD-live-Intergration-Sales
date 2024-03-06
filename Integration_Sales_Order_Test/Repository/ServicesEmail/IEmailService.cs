
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Integration_Sales_Order_Test.Helpers;

namespace Integration_Sales_Order_Test.Repository.ServicesEmail
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }

    
}
