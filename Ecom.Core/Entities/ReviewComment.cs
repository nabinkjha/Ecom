using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECom.Core.Entities
{
    public class ReviewComment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        //Review comment must belong to existing  review.
        public int ReviewId { get; set; }
        public ProductReview Review { get; set; }
        //Review comment must be written by an existing user.
        public int UserId { get; set; }
        public User User { get; set; }

        // A comment can be parent/child of another comment
        public int? ParentCommentId { get; set; }
        public ReviewComment ParentComment { get; set; }
        public ICollection<ReviewComment> ChildComments { get; set; }
        public ICollection<User> LikedByUsers { get; set; }
        //comment can be liked by multiple users
    }
}

