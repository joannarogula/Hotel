namespace HotelBookingApp.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }  // Upewnij się, że ta właściwość istnieje
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}