using Integration_Sales_Order_Test.Model.Client;

namespace Integration_Sales_Order_Test.Repository.ClientServices
{
    public interface IClientServices
    {
        IEnumerable<ClientResponse> GetAllClients();

        ClientResponse GetClientById(int id);

        ClientResponse CreateClient(ClientRequest model);

        ClientResponse ModifyClient(int id, ClientRequest model);

        void RemoveClient(int id, ClientRequest model);
    }
}
