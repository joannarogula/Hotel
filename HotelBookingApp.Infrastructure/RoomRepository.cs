// using HotelBookingApp.Domain.Interfaces;
// using HotelBookingApp.Domain.Models;
// using HotelBookingApp.Infrastructure.Data;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Threading.Tasks;
//
// using System.Threading.Tasks;
// using Microsoft.Extensions.Configuration;
// using MySql.Data.MySqlClient;
//
//
// public class RoomRepository : IRoomRepository
// {
//     private readonly AppDbContext _context;
//
//     public RoomRepository(AppDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<Room> GetRoomByIdAsync(int roomId)
//     {
//         return await _context.Rooms.FindAsync(roomId);
//     }
//
//     public async Task<IEnumerable<Room>> GetAllRoomsAsync()
//     {
//         return await _context.Rooms.ToListAsync();
//     }
//
//     public async Task AddRoomAsync(Room room)
//     {
//         _context.Rooms.Add(room);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task UpdateRoomAsync(Room room)
//     {
//         _context.Rooms.Update(room);
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task DeleteRoomAsync(int roomId)
//     {
//         var room = await GetRoomByIdAsync(roomId);
//         if (room != null)
//         {
//             _context.Rooms.Remove(room);
//             await _context.SaveChangesAsync();
//         }
//     }
// }

using HotelBookingApp.Domain.Interfaces;
using HotelBookingApp.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RoomRepository : IRoomRepository
{
    private readonly string _connectionString;

    public RoomRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<Room> GetRoomByIdAsync(int roomId)
    {
        Room room = null;
        var query = "SELECT RoomId, RoomNumber, Capacity FROM Rooms WHERE RoomId = @RoomId";

        using (var connection = new MySqlConnection(_connectionString))
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@RoomId", roomId);
            await connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    room = new Room
                    {
                        RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                        Number = reader.GetString(reader.GetOrdinal("RoomNumber")),
                        Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                    };
                }
            }
        }
        return room;
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        var rooms = new List<Room>();
        var query = "SELECT RoomId, RoomNumber, Capacity FROM Rooms";

        using (var connection = new MySqlConnection(_connectionString))
        using (var command = new MySqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var room = new Room
                    {
                        RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                        Number = reader.GetString(reader.GetOrdinal("RoomNumber")),
                        Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                    };
                    rooms.Add(room);
                }
            }
        }
        return rooms;
    }

    public async Task AddRoomAsync(Room room)
    {
        var query = "INSERT INTO Rooms (RoomNumber, Capacity) VALUES (@RoomNumber, @Capacity)";

        using (var connection = new MySqlConnection(_connectionString))
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@RoomNumber", room.Number);
            command.Parameters.AddWithValue("@Capacity", room.Capacity);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateRoomAsync(Room room)
    {
        var query = "UPDATE Rooms SET RoomNumber = @RoomNumber, Capacity = @Capacity WHERE RoomId = @RoomId";

        using (var connection = new MySqlConnection(_connectionString))
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@RoomNumber", room.Number);
            command.Parameters.AddWithValue("@Capacity", room.Capacity);
            command.Parameters.AddWithValue("@RoomId", room.RoomId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        var query = "DELETE FROM Rooms WHERE RoomId = @RoomId";

        using (var connection = new MySqlConnection(_connectionString))
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@RoomId", roomId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
