using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.DTOs.request
{
    public class ArtistaRequestDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Genere { get; set; }
        [Required]
        public string Biografia { get; set; }
    }
}
