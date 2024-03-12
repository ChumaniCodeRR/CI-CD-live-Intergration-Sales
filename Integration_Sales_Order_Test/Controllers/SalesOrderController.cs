using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Integration_Sales_Order_Test.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Integration_Sales_Order_Test.Repository;
using System.Reflection;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Dynamic;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace Integration_Sales_Order_Test.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrder _salesOrder;
        private readonly IHostEnvironment _hostingEnvironment;

        public SalesOrderController(IHostEnvironment hostingEnvironment, ISalesOrder salesOrder)
        {
            _salesOrder = salesOrder;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("GetAllSalesOrder")]
        public IActionResult GetAllSalesOrder()
        {
            try
            {
                var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

                var fullPath = Path.Combine(rootPath, "JsonFiles/SO.json"); //combine the root path with that of our json file inside mydata directory

                var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

                if (string.IsNullOrWhiteSpace(jsonData)) return null; //if no data is present then return null or error if you wish

                var orders = JsonConvert.DeserializeObject<SalesOrder>(jsonData); //deserialize object as a list of users in accordance with your json file

                return Ok(orders);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
        [HttpGet]
        [Route("GenerateExpectFileForSalesOrder")]
        public IActionResult GenerateExpectFileForSalesOrder()
        {
            try
            {
                string contentstring = "";
                dynamic jsonRoot = new JObject();

                var rootPath = _hostingEnvironment.ContentRootPath;
                var path = Path.Combine(rootPath, "JsonFiles/SO.json");
                var inputData = System.IO.File.ReadAllText(path);

                JObject o = JObject.Parse(inputData);

                var data = o["SalesOrderRequest"];

                if (data != null)
                {
                    var fieldInfo = data["SalesOrder"];

                    if (fieldInfo != null)
                    {
                        foreach (var token in fieldInfo)
                        {
                            var json = (dynamic)token;

                            json = new JObject();

                            json.orderRef = fieldInfo["salesOrderRef"].ToString();
                            json.orderDate = fieldInfo["orderDate"].ToString();
                            json.currency = fieldInfo["currency"].ToString();
                            json.shipDate = fieldInfo["shipDate"].ToString();
                            json.categoryCode = fieldInfo["categoryCode"].ToString();

                            var addresess = data["SalesOrder"].SelectToken("addresses");

                            if (addresess != null)
                            {
                                foreach (var address in addresess)
                                {
                                    json.addressess = new JObject(
                                             new JProperty("addressType", address["addressType"]),
                                             new JProperty("locationNumber", address["locationNumber"]),
                                             new JProperty("contactName", address["contactName"]),
                                             new JProperty("addressLine1", address["addressLine1"]),
                                             new JProperty("addressCity", address["city"])
                                         ).ToString();
                                }
                            }

                            var orderLines = data["SalesOrder"].SelectToken("orderLines");

                            if (orderLines != null)
                            {
                                foreach (var line in orderLines)
                                {
                                    json.lines = new JObject(
                                             new JProperty("sku", line["skuCode"]),
                                             new JProperty("qty", line["quantity"]),
                                             new JProperty("desc", line["description"])
                                        ).ToString();
                                }
                            }

                            contentstring = JsonConvert.SerializeObject(json);
                        }
                    }
                }

                string filename = "ExpectedSO";

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contentstring);
                var output = new FileContentResult(bytes, "application/octet-stream");
                output.FileDownloadName = filename + ".json";

                System.IO.File.WriteAllBytes(@"C:\Users\s2100170\source\repos\Integration_Sales_Order_Test\Integration_Sales_Order_Test\wwwroot\Files\" + filename + ".json", bytes);

                return Ok(contentstring);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

        }


        [HttpPost("FileUpload")]
        public async Task<ActionResult> UploadFileAsync(IFormFile file)
        {
            var baseUrl = $"https://{Request.Host.Value}{Request.PathBase.Value}";

            var response = await _salesOrder.UploadAsync(baseUrl, file);

            return Ok(response);
        }

        [HttpGet("DownloadOrderSales/{orderRef}")]
        public FileContentResult DownloadOrderSales(string orderRef)
        {
            string contentstring = "";
            var rootPath = _hostingEnvironment.ContentRootPath;

            string filename = "ExpectedSO";
            string outputfile = "";
            /*read from file folder for test json*/
            var fullPath = Path.Combine(rootPath, "wwwroot\\Files\\" + filename + ".json");

            var jsonData = System.IO.File.ReadAllText(fullPath);
            var data = JsonConvert.DeserializeObject<dynamic>(jsonData);

            if (data != null)
            {
                foreach (var item in data["order"])
                {
                    var order = item["orderRef"].ToString();

                    if (order == orderRef)
                    {
                        var itemcount = item.ToString();
                        contentstring = itemcount;
                        outputfile = "(" + item["orderRef"] + "--" + item["categoryCode"] + "--" + System.DateTime.Today.ToString("ddMMyyyy") + ")";
                    }
                }
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(contentstring);
            var output = new FileContentResult(bytes, "application/octet-stream");
            output.FileDownloadName = outputfile + ".json";
            /*Download to wwwroot/file folder*/
            System.IO.File.WriteAllBytes(@"C:\Users\s2100170\source\repos\Integration_Sales_Order_Test\Integration_Sales_Order_Test\wwwroot\Files\" + outputfile + ".json", bytes);

            return output;
        }
    }
}
