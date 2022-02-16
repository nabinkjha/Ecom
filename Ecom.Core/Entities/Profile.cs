using System;
using System.ComponentModel.DataAnnotations;

namespace ECom.Core.Entities
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}