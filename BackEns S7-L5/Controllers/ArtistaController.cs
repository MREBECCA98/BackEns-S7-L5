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
    public class ArtistaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistaController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtisti()
        {
            var artisti = await _context.Artisti
                .Select(a => new ArtistaResponseDto
                {
                    ArtistaId = a.ArtistaId,
                    Nome = a.Nome,
                    Genere = a.Genere,
                    Biografia = a.Biografia
                })
                .ToListAsync();

            return Ok(artisti);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);

            if (artista == null)
                return NotFound();

            var dto = new ArtistaResponseDto
            {
                ArtistaId = artista.ArtistaId,
                Nome = artista.Nome,
                Genere = artista.Genere,
                Biografia = artista.Biografia
            };

            return Ok(dto);
        }


        [HttpPost]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> CreateArtista([FromBody] ArtistaRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var artista = new Artista
            {
                Nome = dto.Nome,
                Genere = dto.Genere,
                Biografia = dto.Biografia
            };

            _context.Artisti.Add(artista);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArtista), new { id = artista.ArtistaId }, dto);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> UpdateArtista(int id, [FromBody] ArtistaRequestDto dto)
        {
            var artista = await _context.Artisti.FindAsync(id);

            if (artista == null)
                return NotFound();

            artista.Nome = dto.Nome;
            artista.Genere = dto.Genere;
            artista.Biografia = dto.Biografia;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Artista modificato con successo" });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> DeleteArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);
            if (artista == null)
                return NotFound();

            _context.Artisti.Remove(artista);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Artista eliminato con successo" });
        }
    }
}
