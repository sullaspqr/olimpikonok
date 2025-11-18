using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OlimpikonokAPI.Models;

namespace OlimpikonokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrszagController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    List<Orszag> orszagok = context.Orszags.ToList();
                    return Ok(orszagok);
                }
                catch (Exception ex)
                {
                    List<Orszag> valasz = new List<Orszag>();
                    Orszag hiba = new Orszag() {
                        Id = -1,
                        Nev = $"Hiba a betöltés során: {ex.Message}"
                    };
                    valasz.Add(hiba);
                    return BadRequest(valasz);
                }
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    Orszag eredmeny = context.Orszags.FirstOrDefault(o =>  o.Id == id);
                    if (eredmeny != null)
                        return Ok(eredmeny);
                    else
                    {
                        Orszag hiba = new Orszag()
                        {
                            Id = -1,
                            Nev = $"Nincs ilyen azonosítójú ország"
                        };
                        return NotFound(hiba);
                    }
                }
                catch (Exception ex)
                {
                    Orszag hiba = new Orszag()
                    {
                        Id = -1,
                        Nev = $"Hiba a betöltés közben: {ex.Message}"
                    };
                    return BadRequest(hiba);
                }
            }
        }

        [HttpPost("UjOrszag")]
        public IActionResult PostOrszag(Orszag orszag) 
        { 
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    context.Orszags.Add(orszag);
                    context.SaveChanges();
                    return Ok("Sikers rögzítés.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Hiba a rögzítés közben: {ex.Message}");
                }
            }
        }

        [HttpPut("ModositOrszag")]
        public IActionResult PutOrszag(Orszag orszag)
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    
                    if (context.Orszags.Select(o => o.Id).Contains(orszag.Id))
                        {
                        context.Orszags.Update(orszag);
                        context.SaveChanges();
                        return Ok("Sikeres módosítás.");
                    }
                    else
                    {
                        return NotFound("Nincs ilyen ország.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Hiba a módosítás közben: {ex.Message}");
                }
            }
        }

        [HttpDelete("TorolOrszag")]
        public IActionResult DeleteOrszag(int id)
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {

                    if (context.Orszags.Select(o => o.Id).Contains(id))
                    {
                        Orszag torlendo = new Orszag { Id = id };
                        context.Orszags.Remove(torlendo);
                        context.SaveChanges();
                        return Ok("Sikeres törlés.");
                    }
                    else
                    {
                        return NotFound("Nincs ilyen ország.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Hiba a törlés közben: {ex.Message}");
                }
            }
        }
    }
}
