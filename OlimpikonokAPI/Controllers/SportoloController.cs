using Microsoft.AspNetCore.Http;
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
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    List<Sportolo> sportolok = context.Sportolos.Include(s => s.Orszag).Include(s => s.Sportag).ToList();
                    return Ok(sportolok);
                }
                catch (Exception ex)
                {
                    List<Sportolo> valasz = new List<Sportolo>();
                    Sportolo hiba = new Sportolo()
                    {
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
                    Sportolo eredmeny = context.Sportolos.FirstOrDefault(o => o.Id == id);
                    if (eredmeny != null)
                        return Ok(eredmeny);
                    else
                    {
                        Sportolo hiba = new Sportolo()
                        {
                            Id = -1,
                            Nev = $"Nincs ilyen azonosítójú sportoló"
                        };
                        return NotFound(hiba);
                    }
                }
                catch (Exception ex)
                {
                    Sportolo hiba = new Sportolo()
                    {
                        Id = -1,
                        Nev = $"Hiba a betöltés közben: {ex.Message}"
                    };
                    return BadRequest(hiba);
                }
            }
        }

        [HttpGet("GetAllSportoloOSDTO")]
        public IActionResult GetAllSportoloOSDTO()
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    List<SportoloOSDTO> dtoList = context.Sportolos.
                        Include(s => s.Orszag).
                        Include(s => s.Sportag).
                        Select(s => new SportoloOSDTO() { Id = s.Id, Nev = s.Nev, OrszagNev = s.Orszag.Nev, SportagNev = s.Sportag.Megnevezes }).
                        ToList();
                    return Ok(dtoList);
                    /*
                    List<Sportolo> sportolok = context.Sportolos.Include(s => s.Orszag).Include(s => s.Sportag).ToList();
                    List<SportoloOSDTO> dtoList = new List<SportoloOSDTO>();
                    foreach (Sportolo sportolo in sportolok)
                    {
                        SportoloOSDTO dto = new SportoloOSDTO()
                        {
                            Id = sportolo.Id,
                            Nev = sportolo.Nev,
                            OrszagNev = sportolo.Orszag.Nev,
                            SportagNev = sportolo.Sportag.Megnevezes
                        };
                        dtoList.Add(dto);
                    }
                    return Ok(dtoList);
                    */
                }
                catch (Exception ex)
                {
                    List<SportoloOSDTO> valasz = new List<SportoloOSDTO>();
                    SportoloOSDTO hiba = new SportoloOSDTO()
                    {
                        Id = -1,
                        Nev = $"Hiba a betöltés során: {ex.Message}"
                    };
                    valasz.Add(hiba);
                    return BadRequest(valasz);
                }
            }
        }        

        [HttpPost("UjSportolo")]
        public IActionResult PostSportolo(Sportolo sportolo)
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    context.Sportolos.Add(sportolo);
                    context.SaveChanges();
                    return Ok("Sikeres rögzítés");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Hiba a rögzítés során: {ex.Message}");
                }
            }
        }

        [HttpPut("ModositSportolo")]
        public IActionResult PutSportolo(Sportolo sportolo)
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    if (context.Sportolos.Contains(sportolo))
                    {
                        context.Sportolos.Update(sportolo);
                        context.SaveChanges();
                        return Ok("Sikeres módosítás");
                    }
                    else
                    {
                        return NotFound("Nincs ilyen sportoló");
                    }
                    
                }
                catch (Exception ex)
                {
                    return BadRequest($"Hiba a módosítás során: {ex.Message}");
                }
            }
        }

        [HttpDelete("TorolSportolo")]
        public IActionResult DeleteSportolo(int id)
        {
            using (var context = new OlimpikonokContext())
            {
                try
                {
                    if (context.Sportolos.Select(s => s.Id).Contains(id))
                    {
                        Sportolo sportolo = new Sportolo() { Id = id };
                        context.Sportolos.Remove(sportolo);
                        context.SaveChanges();
                        return Ok("Sikeres törlés");
                    }
                    else
                    {
                        return NotFound("Nincs ilyen sportoló");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest($"Hiba a törlés során: {ex.Message}");
                }
            }
        }
    }
}
