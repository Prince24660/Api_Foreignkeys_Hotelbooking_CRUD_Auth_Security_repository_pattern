using API_Foreignkey.Models;
using API_Foreignkey.ResourceParameter;

namespace API_Foreignkey.IReposiory
{
    public interface ICientRepository
    {
        Task<Client> GetClientAsync(int clientId);
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<IEnumerable<Client>> GetClientsAsync(ClientParameter clientsResourceParameters);
        Task<bool> IsClientExistsAsync(int clientId);
        void UpdateClient(Client client);
    }
}
