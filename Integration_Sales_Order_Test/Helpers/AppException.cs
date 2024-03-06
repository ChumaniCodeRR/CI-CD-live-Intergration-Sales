
using System;
using System.Globalization;

namespace Integration_Sales_Order_Test.Helpers
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args)
         : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
