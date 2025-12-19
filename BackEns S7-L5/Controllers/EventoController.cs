using BackEns_S7_L5.Models.DTOs.request;
using BackEns_S7_L5.Models.DTOs.response;
using BackEns_S7_L5.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEns_S7_L5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventoController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventi()
        {
            var eventi = await _context.Eventi
                .Include(e => e.Artista)
                .Select(e => new EventoResponseDto
                {
                    EventoId = e.EventoId,
                    Titolo = e.Titolo,
                    Data = e.Data,
                    Luogo = e.Luogo,
                    ArtistaId = e.ArtistaId,

                })
                .ToListAsync();

            return Ok(eventi);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEvento(int id)
        {
            var evento = await _context.Eventi
                .Include(e => e.Artista)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (evento == null)
                return NotFound();

            var dto = new EventoResponseDto
            {
                EventoId = evento.EventoId,
                Titolo = evento.Titolo,
                Data = evento.Data,
                Luogo = evento.Luogo,
                ArtistaId = evento.ArtistaId,

            };

            return Ok(dto);
        }


        [HttpPost]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> CreateEvento([FromBody] EventoRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var artista = await _context.Artisti.FindAsync(dto.ArtistaId);
            if (artista == null)
                return BadRequest(new { message = "Artista non trovato" });

            var evento = new Evento
            {
                Titolo = dto.Titolo,
                Data = dto.Data,
                Luogo = dto.Luogo,
                ArtistaId = dto.ArtistaId
            };

            _context.Eventi.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, dto);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> UpdateEvento(int id, [FromBody] EventoRequestDto dto)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null)
                return NotFound();

            var artista = await _context.Artisti.FindAsync(dto.ArtistaId);
            if (artista == null)
                return BadRequest(new { message = "Artista non trovato" });

            evento.Titolo = dto.Titolo;
            evento.Data = dto.Data;
            evento.Luogo = dto.Luogo;
            evento.ArtistaId = dto.ArtistaId;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Evento modificato con successo" });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null)
                return NotFound();

            _context.Eventi.Remove(evento);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Evento eliminato con successo" });
        }
    }
}
