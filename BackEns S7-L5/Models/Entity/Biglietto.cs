using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEns_S7_L5.Models.Entity
{
    public class Biglietto
    {
        [Key]
        public int BigliettoId { get; set; }
        [Required]
        public DateTime DataAcquisto { get; set; }


        public int EventoId { get; set; }
        [ForeignKey("EventoId")]
        public Evento Evento { get; set; }


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


    }
}
