using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.DTOs.request
{
    public class BigliettoRequestDto
    {
        [Required]
        public int EventoId { get; set; }
        [Required]
        public int Quantita { get; set; } = 1;
    }
}
