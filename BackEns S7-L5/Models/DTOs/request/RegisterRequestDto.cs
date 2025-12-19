using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.DTOs.request
{
    public class RegisterRequestDto
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cognome { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
    }
}
