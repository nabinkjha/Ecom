using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECom.Core.Entities
{
    public class ProductReview
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        //Review must be for a product
        public Product Product { get; set; }
        public int ProductId { get; set; }
        //Review must be done by a User
        public User Reviewer { get; set; }
        public int ReviewerId { get; set; }
        //Review can be liked by multiple Users
        public ICollection<User> LikedBy { get; set; }
        //Review can be have multiple comments
        public ICollection<ReviewComment> Comments { get; set; }
    }
}
