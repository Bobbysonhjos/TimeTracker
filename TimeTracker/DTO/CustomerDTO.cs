using System.ComponentModel.DataAnnotations;

namespace TimeTracker.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name can only contain a maximum of 50 characters")]
        public string Name { get; set; }
    }
}
