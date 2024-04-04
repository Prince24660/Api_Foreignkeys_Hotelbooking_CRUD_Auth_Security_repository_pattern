using API_Foreignkey.Models;
using API_Foreignkey.Models.ModelVM;
using API_Foreignkey.ResourceParameter;
using Microsoft.EntityFrameworkCore;
using Test2.Data;

namespace API_Foreignkey.IReposiory.Repository
{
    public class RoomRepository : IRoomRepository

    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context, IBookingRepository bookingRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;

        }
        public async Task<Room> GetRoomAsync(int roomId)
        {
            return await _context.rooms.FirstOrDefaultAsync(a => a.Id == roomId);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _context.rooms.ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(RoomResourceParameter roomsResourceParameters)
        {
           if(roomsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(roomsResourceParameters));
            }
            if (roomsResourceParameters.HasBalcony == null
                 && roomsResourceParameters.RoomType == null
                 && roomsResourceParameters.PriceLessThan == null
                 && roomsResourceParameters.NumberOfPerson == null
               && string.IsNullOrWhiteSpace(roomsResourceParameters.SearchQuery))   
            {
                return await GetRoomsAsync();
            }
            var collection =_context.rooms as IQueryable<Room>;
            if (!(roomsResourceParameters.HasBalcony == null))
            {
                collection = collection.Where(r => r.HasBalcony == roomsResourceParameters.HasBalcony);
            }

            if (!(roomsResourceParameters.RoomType == null))
            {
                var roomType = roomsResourceParameters.RoomType;
                collection = collection.Where(r => r.Type == roomType);
            }

            if (!(roomsResourceParameters.PriceLessThan == null))
            {
                collection = collection.Where(r => r.PriceForDay < roomsResourceParameters.PriceLessThan);
            }

            if (!(roomsResourceParameters.NumberOfPerson == null))
            {
                collection = collection.Where(r => r.MaxNumberOfPerson == roomsResourceParameters.NumberOfPerson);
            }

            if (!(roomsResourceParameters.VacancyInDays == null))
            {
                if (!_bookingRepository.AreDatesCorrect(roomsResourceParameters.VacancyInDays))
                {
                    return null;
                }

                var roomsIds = await GetVacancyRoomsAsync(roomsResourceParameters.VacancyInDays);
                collection = collection.Where(r => roomsIds.Contains(r.Id));
            }

            if (!string.IsNullOrWhiteSpace(roomsResourceParameters.SearchQuery))
            {

                var searchQuery = roomsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(r => r.Description.Contains(searchQuery));
            }

            return await collection.ToListAsync();
        }
      
        private async Task<IEnumerable<int>> GetVacancyRoomsAsync(DatesVM dates)
        {
            var rooms = await GetRoomsAsync();
            List<int> roomsIds = new List<int>();

            foreach (var room in rooms)
            {
                if (await _bookingRepository.IsRoomVacancyAsync(room.Id, dates))
                {
                    roomsIds.Add(room.Id);
                }
            }

            return roomsIds;
        }
        public async Task<bool> IsRoomExistsAsync(int roomId)
        {
            return await _context.rooms.AnyAsync(r => r.Id == roomId);
        }

        public void UpdateRoom(Room room)
        {
            throw new NotImplementedException();
        }
    }
}
