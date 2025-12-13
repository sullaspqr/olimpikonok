using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlimpikonokAPI.DTOs;
using OlimpikonokAPI.Models;

namespace OlimpikonokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportoloController : ControllerBase
    {
        private readonly OlimpikonokContext _context;

        public SportoloController(OlimpikonokContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var sportolok = _context.Sportolos
                    .Include(s => s.Orszag)
                    .Include(s => s.Sportag)
                    .ToList();

                return Ok(sportolok);
            }
            catch (Exception ex)
            {
                var hiba = new Sportolo
                {
                    Id = -1,
                    Nev = $"Hiba a betöltés során: {ex.Message}"
                };
                return BadRequest(new[] { hiba });
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var eredmeny = _context.Sportolos.FirstOrDefault(s => s.Id == id);
                if (eredmeny != null)
                    return Ok(eredmeny);

                return NotFound(new Sportolo
                {
                    Id = -1,
                    Nev = "Nincs ilyen azonosítójú sportoló"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Sportolo
                {
                    Id = -1,
                    Nev = $"Hiba a betöltés közben: {ex.Message}"
                });
            }
        }

        [HttpGet("GetAllSportoloOSDTO")]
        public IActionResult GetAllSportoloOSDTO()
        {
            try
            {
                var dtoList = _context.Sportolos
                    .Include(s => s.Orszag)
                    .Include(s => s.Sportag)
                    .Select(s => new SportoloOSDTO
                    {
                        Id = s.Id,
                        Nev = s.Nev,
                        OrszagNev = s.Orszag.Nev,
                        SportagNev = s.Sportag.Megnevezes
                    })
                    .ToList();

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                var hiba = new SportoloOSDTO
                {
                    Id = -1,
                    Nev = $"Hiba a betöltés során: {ex.Message}"
                };
                return BadRequest(new[] { hiba });
            }
        }

        [HttpPost("UjSportolo")]
        public IActionResult PostSportolo(Sportolo sportolo)
        {
            try
            {
                _context.Sportolos.Add(sportolo);
                _context.SaveChanges();
                return Ok("Sikeres rögzítés");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a rögzítés során: {ex.Message}");
            }
        }

        [HttpPut("ModositSportolo")]
        public IActionResult PutSportolo(Sportolo sportolo)
        {
            try
            {
                var letezo = _context.Sportolos.Any(s => s.Id == sportolo.Id);
                if (!letezo)
                    return NotFound("Nincs ilyen sportoló");

                _context.Sportolos.Update(sportolo);
                _context.SaveChanges();
                return Ok("Sikeres módosítás");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a módosítás során: {ex.Message}");
            }
        }

        [HttpDelete("TorolSportolo")]
        public IActionResult DeleteSportolo(int id)
        {
            try
            {
                var letezo = _context.Sportolos.Any(s => s.Id == id);
                if (!letezo)
                    return NotFound("Nincs ilyen sportoló");

                var sportolo = new Sportolo { Id = id };
                _context.Sportolos.Remove(sportolo);
                _context.SaveChanges();
                return Ok("Sikeres törlés");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a törlés során: {ex.Message}");
            }
        }
    }
}
