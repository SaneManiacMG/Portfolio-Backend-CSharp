using Portfolio.Backend.Csharp.Models.Enums;

namespace Portfolio.Backend.Csharp.Models.Entities
{
    public class Login : UserBase
    {

        public Login(string userId, string password, DateTime createDate)
        {
            UserId = userId;
            Password = password;
            AccountStatus = AccountStatus.Unverified;
            DateCreated = createDate;
            DateModified = createDate;
        }

        public Login()
        {
        }

        public string Password { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
