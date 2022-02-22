using System.ComponentModel.DataAnnotations;

namespace ECom.Core.Entities
{
    public  class ApiClient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public bool IsBlocked { get; set; }
    }
}
