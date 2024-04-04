using API_Foreignkey.IReposiory;
using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using API_Foreignkey.ResourceParameter;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Foreignkey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IDBRepository _dbRepository;
        private readonly IMapper _mapper;
        public RoomController(IRoomRepository roomRepository, IDBRepository dBRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _dbRepository = dBRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<RoomVM>> NewRoom(RoomForCreateionVM room)
        {
            string formattedDescription = room.Description?.Replace(" ", "\n");
            try
            {
                // Replace newline characters with space before saving to the database
                room.Description = room.Description?.Replace("\n", " ");

                var newroom = _mapper.Map<Room>(room);
                _dbRepository.Add(newroom);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetRoom), new { roomId = newroom.Id }, _mapper.Map<RoomVM>(newroom));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<RoomVM>> GetAllRoomBoked()
        {
            var roomboked = await _roomRepository.GetRoomsAsync();
            return Ok(roomboked);
        }

        [HttpGet("{roomId}")]
        public async Task<ActionResult<RoomVM>> GetRoom(int roomId)
        {
            try
            {
                var room =await _roomRepository.GetRoomAsync(roomId);
                if (room != null)
                {
                    return Ok(_mapper.Map<RoomVM>(room));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return NotFound();
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<RoomVM>>> GetRooms([FromQuery] RoomResourceParameter roomResourceParameter)
        //{
        //    try
        //    {
        //        var rooms = _roomRepository.GetRoomsAsync(roomResourceParameter);
        //        if (rooms == null)
        //            return BadRequest();
        //        if (rooms != null)
        //            return Ok(_mapper.Map<IEnumerable<RoomVM>>(rooms));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
        //    }
        //    return BadRequest();
        //}
    }
}
