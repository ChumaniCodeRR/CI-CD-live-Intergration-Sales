using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Integration_Sales_Order_Test.Model
{
    public class RootObject
    {
        [JsonProperty("order")]
        public List<Order> DataResults { get; set; }

    }
}
