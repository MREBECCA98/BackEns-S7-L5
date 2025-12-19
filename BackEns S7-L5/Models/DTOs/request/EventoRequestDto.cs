using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.DTOs.request
{
    public class EventoRequestDto
    {
        [Required]
        public string Titolo { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public string Luogo { get; set; }
        [Required]
        public int ArtistaId { get; set; }
    }
}
