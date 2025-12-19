using System.ComponentModel.DataAnnotations;

namespace BackEns_S7_L5.Models.DTOs.response
{
    public class ArtistaResponseDto
    {

        public int ArtistaId { get; set; }

        public string Nome { get; set; }

        public string Genere { get; set; }

        public string Biografia { get; set; }
    }
}
