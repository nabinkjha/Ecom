using System;
using System.ComponentModel.DataAnnotations;

namespace ECom.Core.Entities
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}