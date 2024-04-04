using API_Foreignkey.Models;
using API_Foreignkey.ResourceParameter;

namespace API_Foreignkey.IReposiory
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomAsync(int roomId);
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<IEnumerable<Room>> GetRoomsAsync(RoomResourceParameter roomsResourceParameters);
        Task<bool> IsRoomExistsAsync(int roomId);
        void UpdateRoom(Room room);
    }
}
