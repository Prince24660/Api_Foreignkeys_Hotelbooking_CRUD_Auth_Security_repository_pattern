using API_Foreignkey.IReposiory;
using API_Foreignkey.IReposiory.Repository;
using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_Foreignkey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ICientRepository _cientRepository;
        private readonly IMapper _mapper;
        private readonly IDBRepository _dBRepository;
        public ClientController(ICientRepository cientRepository, IMapper mapper, IDBRepository dBRepository)
        {
            _cientRepository = cientRepository;
            _mapper = mapper;
            _dBRepository = dBRepository;
        }
        [HttpPost]
        public async Task<ActionResult<ClientVM>> NewClient(ClientForCreateionVM clientforcreateionvm)
        {
            try
            {
                var newclient = _mapper.Map<Client>(clientforcreateionvm);
                _dBRepository.Add(newclient);
                if(await _dBRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetClient), new {clientid =newclient.Id}, _mapper.Map<ClientVM>(newclient));
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            
        }
            return BadRequest();
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ClientVM>>> GetAllClient()
        //{
        //    var clientall = _cientRepository.GetClientsAsync();
        //    return Ok(clientall);
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientVM>>> GetAllClient()
        {   
            var clientall = await _cientRepository.GetClientsAsync();

            var clientVMs = clientall.Select(client => new ClientVM
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Sex = client.Sex,
                Age = client.Age,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                Address = new AddressVM
                {
                    City = client.Address.City,
                    Street = client.Address.Street,
                    HouseNumber = client.Address.HouseNumber,
                    PostCode = client.Address.PostCode
                }
            });

            return Ok(clientVMs);
        }


        [HttpGet("{clientId}")]
        public async Task<ActionResult<ClientVM>> GetClient(int clientId)
        {
            try
            {
                var client = await _cientRepository.GetClientAsync(clientId);
                if (client != null)
                {
                    return Ok(_mapper.Map<ClientVM>(client));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }

    }
  
}
