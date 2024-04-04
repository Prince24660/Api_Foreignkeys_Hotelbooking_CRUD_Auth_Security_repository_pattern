using API_Foreignkey.Models;
using API_Foreignkey.ResourceParameter;
using Microsoft.EntityFrameworkCore;
using Test2.Data;

namespace API_Foreignkey.IReposiory.Repository
{
    public class ClientRepository : ICientRepository
    {
        private readonly ApplicationDbContext _context;
        public ClientRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Client> GetClientAsync(int clientId)
        {
            return await _context.clients.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _context.clients.Include(a => a.Address).ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync(ClientParameter clientsResourceParameters)
        {
            if (clientsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(clientsResourceParameters));
            }

            if (clientsResourceParameters.Sex == null
                 && string.IsNullOrWhiteSpace(clientsResourceParameters.SearchQuery))
            {
                return await GetClientsAsync();
            }

            var collection = _context.clients.Include(c => c.Address) as IQueryable<Client>;

            if (!(clientsResourceParameters.Sex == null))
            {
                var sex = clientsResourceParameters.Sex;
                collection = collection.Where(c => c.Sex == sex);
            }

            if (!string.IsNullOrWhiteSpace(clientsResourceParameters.SearchQuery))
            {

                var searchQuery = clientsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(c => c.FirstName.Contains(searchQuery)
                    || c.LastName.Contains(searchQuery)
                    || c.LastName.Contains(searchQuery)
                    || c.PhoneNumber.Contains(searchQuery)
                    || c.Email.Contains(searchQuery));
            }

            return await collection.ToListAsync();
        }

        public async Task<bool> IsClientExistsAsync(int clientId)
        {
            return await _context.clients.AnyAsync(c=>c.Id== clientId);
        }

        public void UpdateClient(Client client)
        {
            //throw new NotImplementedException();
        }
    }
}
