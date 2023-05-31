using Portfolio.Backend.Csharp.Models.User.Requests;
using Portfolio.Backend.Csharp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Csharp.Models.Entities
{
    public class User : UserBase
    {
        public User(string generatedUserId, UserRequest newUserDetails, DateTime createDate)
        {
            UserId = generatedUserId;
            Username = newUserDetails.Username;
            FirstName = newUserDetails.FirstName;
            LastName = newUserDetails.LastName;
            Email = newUserDetails.Email;
            PhoneNr = newUserDetails.PhoneNr;
            Role = Role.Visitor;
            DateCreated = createDate;
            DateModified = createDate;
        }

        public User()
        {
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public Role Role { get; set; }
    }
}
