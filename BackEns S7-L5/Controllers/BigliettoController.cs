using BackEns_S7_L5.Models.DTOs.request;
using BackEns_S7_L5.Models.DTOs.response;
using BackEns_S7_L5.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BackEns_S7_L5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BigliettoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BigliettoController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AcquistaBiglietto([FromBody] BigliettoRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var evento = await _context.Eventi.FindAsync(dto.EventoId);
            if (evento == null)
                return BadRequest(new { message = "Evento non trovato" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            for (int i = 0; i < dto.Quantita; i++)
            {
                var biglietto = new Biglietto
                {
                    EventoId = dto.EventoId,
                    UserId = userId,
                    DataAcquisto = DateTime.UtcNow
                };
                _context.Biglietti.Add(biglietto);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"{dto.Quantita} biglietto acquistato con successo" });
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBiglietti()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var query = _context.Biglietti.Include(b => b.Evento).AsQueryable();

            if (!roles.Contains("Amministratore"))
            {
                query = query.Where(b => b.UserId == userId);
            }

            var biglietti = await query
                .Select(b => new BigliettoResponseDto
                {
                    BigliettoId = b.BigliettoId,
                    EventoId = b.EventoId,
                    TitoloEvento = b.Evento.Titolo,
                    DataAcquisto = b.DataAcquisto,
                })
                .ToListAsync();

            return Ok(biglietti);
        }
    }
}
