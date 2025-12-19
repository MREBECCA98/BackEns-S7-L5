using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.DTOs.request
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
