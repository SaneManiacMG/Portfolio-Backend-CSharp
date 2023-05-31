using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Csharp.Models.Entities
{
    public abstract class UserBase
    {
        [Key]
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
