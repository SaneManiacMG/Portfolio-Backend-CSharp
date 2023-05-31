using Portfolio.Backend.Csharp.Models.Enums;

namespace Portfolio.Backend.Csharp.Models.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public Role Role { get; set; }
    }
}
