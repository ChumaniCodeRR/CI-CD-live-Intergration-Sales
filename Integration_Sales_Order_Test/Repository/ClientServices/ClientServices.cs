using AutoMapper;
using Integration_Sales_Order_Test.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Integration_Sales_Order_Test.Entities;
using Integration_Sales_Order_Test.Model.Client;
using Integration_Sales_Order_Test.Repository.ServicesEmail;

namespace Integration_Sales_Order_Test.Repository.ClientServices
{
    public class ClientServices : IClientServices
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public ClientServices(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }


        public ClientResponse CreateClient(ClientRequest model)
        {
            if (_context.Client.Any(x => x.ClientName == model.ClientName))
                throw new AppException($"Client Name '{ model.ClientName }' is already registered");

            var client = _mapper.Map<Client>(model);

            _context.Client.Add(client);
            _context.SaveChanges();

            return _mapper.Map<ClientResponse>(client);
        }

        public ClientResponse ModifyClient(int id, ClientRequest model)
        {
            var client = getClient(id);

            if (client.ClientName != model.ClientName && _context.Client.Any(x => x.ClientName == model.ClientName))
                throw new AppException($"Client name '{ model.ClientName }' is already taken");

            _mapper.Map(model, client);
            _context.Client.Update(client);
            _context.SaveChanges();

            return _mapper.Map<ClientResponse>(client);

        }
        public IEnumerable<ClientResponse> GetAllClients()
        {
            var clients = _context.Products;
            return _mapper.Map<IList<ClientResponse>>(clients);
        }

        public ClientResponse GetClientById(int id)
        {
            var product = getClient(id);
            return _mapper.Map<ClientResponse>(product);
        }

        public void RemoveClient(int id, ClientRequest model)
        {
            var client = getClient(id);
            _context.Client.Remove(client);
            _context.SaveChanges();
        }

        private Client getClient(int id)
        {
            var client = _context.Client.Find(id);
            if (client == null) throw new KeyNotFoundException("Client not found");
            return client;
        }
    }
}
