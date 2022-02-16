using System.Collections.Generic;

namespace ECom.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Profile Profile { get; set; }  // User can have at most one profile
        public ICollection<User> Friends { get; set; }//User can have multiple friends
        public ICollection<User> FriendsOf { get; set; } // User can be friend of multiple users
        public ICollection<ProductReview> ProductReviews { get; set; }  //User can create multiple reviews
        public ICollection<ProductReview> LikedProductReviews { get; set; }   //User can like multiple reviews
        public ICollection<ReviewComment> ReviewComments { get; set; }  //User can add multiple review comments
                                                                        // public ICollection<ReviewComment> LikedComments { get; set; }     //User can like multiple review comments

    }
}
