using System.ComponentModel.DataAnnotations;


namespace PropertyManagement.Models
{
    public class SearchPropertyRequest
    {
        [Required]
        public string Query { get; set; }
        public string Market { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
    }
}