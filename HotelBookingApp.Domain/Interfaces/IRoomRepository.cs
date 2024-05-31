namespace HotelBookingApp.Domain.Interfaces;
using HotelBookingApp.Domain.Models;

public interface IRoomRepository
{
    Task<Room> GetRoomByIdAsync(int roomId);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<IEnumerable<Room>> GetReservedRoomsAsync(DateTime startDate, DateTime endDate);
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(int roomId);
}

