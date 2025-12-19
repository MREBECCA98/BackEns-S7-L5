using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cognome { get; set; }

        public DateTime CreateAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime Birthday { get; set; }
    }
}
