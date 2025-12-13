using Microsoft.AspNetCore.Mvc;
using OlimpikonokAPI.Models;

namespace OlimpikonokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrszagController : ControllerBase
    {
        private readonly OlimpikonokContext _context;

        // DI a DbContext-hez
        public OrszagController(OlimpikonokContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var orszagok = _context.Orszags.ToList();
                return Ok(orszagok);
            }
            catch (Exception ex)
            {
                var hiba = new Orszag
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
                var eredmeny = _context.Orszags.FirstOrDefault(o => o.Id == id);
                if (eredmeny != null)
                    return Ok(eredmeny);

                return NotFound(new Orszag
                {
                    Id = -1,
                    Nev = "Nincs ilyen azonosítójú ország"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Orszag
                {
                    Id = -1,
                    Nev = $"Hiba a betöltés közben: {ex.Message}"
                });
            }
        }

        [HttpPost("UjOrszag")]
        public IActionResult PostOrszag(Orszag orszag)
        {
            try
            {
                _context.Orszags.Add(orszag);
                _context.SaveChanges();
                return Ok("Sikeres rögzítés.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a rögzítés közben: {ex.Message}");
            }
        }

        [HttpPut("ModositOrszag")]
        public IActionResult PutOrszag(Orszag orszag)
        {
            try
            {
                var letezo = _context.Orszags.Any(o => o.Id == orszag.Id);
                if (!letezo)
                    return NotFound("Nincs ilyen ország.");

                _context.Orszags.Update(orszag);
                _context.SaveChanges();
                return Ok("Sikeres módosítás.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a módosítás közben: {ex.Message}");
            }
        }

        [HttpDelete("TorolOrszag")]
        public IActionResult DeleteOrszag(int id)
        {
            try
            {
                var letezo = _context.Orszags.Any(o => o.Id == id);
                if (!letezo)
                    return NotFound("Nincs ilyen ország.");

                var torlendo = new Orszag { Id = id };
                _context.Orszags.Remove(torlendo);
                _context.SaveChanges();
                return Ok("Sikeres törlés.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba a törlés közben: {ex.Message}");
            }
        }
    }
}

