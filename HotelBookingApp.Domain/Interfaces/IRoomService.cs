namespace HotelBookingApp.Domain.Services;
using HotelBookingApp.Domain.Interfaces;  // Dla IUserRepository, IRoomRepository, IBookingRepository

using HotelBookingApp.Domain.Models;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);
    Task<Room> GetRoomByIdAsync(int roomId);
    Task<Room> CreateRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(int roomId);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
}

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate)
    {
        // Logika filtrująca pokoje na podstawie dostępności
        var rooms = await _roomRepository.GetAllRoomsAsync();
        return rooms.Where(r => r.IsAvailable && NoBookingOverlap(r, startDate, endDate));
    }
    
    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllRoomsAsync();
    }

    public async Task<Room> GetRoomByIdAsync(int roomId)
    {
        return await _roomRepository.GetRoomByIdAsync(roomId);
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        await _roomRepository.AddRoomAsync(room);
        return room;
    }

    public async Task UpdateRoomAsync(Room room)
    {
        await _roomRepository.UpdateRoomAsync(room);
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        await _roomRepository.DeleteRoomAsync(roomId);
    }
    
    
    private bool NoBookingOverlap(Room room, DateTime start, DateTime end)
    {
        // Tu można dodać logikę sprawdzającą, czy nie ma nakładających się rezerwacji
        return true;
    }
}
