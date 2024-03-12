using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Repository.ServicesEmail;
using Integration_Sales_Order_Test.Model.Client;
using Integration_Sales_Order_Test.Repository.ClientServices;
using Integration_Sales_Order_Test.Model.Category;
using Integration_Sales_Order_Test.Repository.CategoryServices;

namespace Integration_Sales_Order_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : BaseController
    {
        private readonly IClientServices _client;
        private readonly IMapper _mapper;

        public ClientController(IClientServices client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        // GET: api/<ClientController>
        [HttpGet]
        public ActionResult<IEnumerable<Client>> GetAllClients()
        {
            var client = _client.GetAllClients();
            return Ok(client);
        }

        // GET api/<ClientController>/5
        [HttpGet("{id}")]
        public ActionResult<ClientResponse> GetClientById(int clientId)
        {
            if (clientId != Client.ClientID)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            var client = _client.GetClientById(clientId);
            return Ok(client);
        }


        // POST api/<ClientController>
        [HttpPost]
        public ActionResult<ClientResponse> CreateClient(ClientRequest model)
        {
            _client.CreateClient(model);
            return Ok(new { message = "Client created successfully" });
        }

        // PUT api/<ClientController>/5
        [HttpPut("{clientId:int}")]
        public ActionResult<ClientResponse> ModifyClient(int clientId, ClientRequest model)
        {
            if (clientId != model.ClientID)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            _client.ModifyClient(clientId, model);

            return Ok(new { message = "Client modified successfully" });
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{clientId:int}")]
        public IActionResult RemoveClient(int clientId, ClientRequest model)
        {
            if (clientId != model.ClientID)
                return Unauthorized(new { message = "Unauthorized" });

            _client.RemoveClient(clientId, model);

            return Ok(new { message = "Client removed successfully" });
        }
    }
}
