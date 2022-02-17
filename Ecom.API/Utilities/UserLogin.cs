using System.ComponentModel.DataAnnotations;

namespace ECom.API.Utilities
{
    public class UserLogin
    {
        [Required]
        [Key]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
