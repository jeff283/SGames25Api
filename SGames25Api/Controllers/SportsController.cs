using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGames25Api.Data;
using SGames25Api.Models;
using SGames25Api.Models.DTOs;

namespace SGames25Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly SummerGamesContext _context;

        public SportsController(SummerGamesContext context)
        {
            _context = context;
        }

        // GET: api/Sports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SportDTO>>> GetSports()
        {
            // return await _context.Sports.ToListAsync();
            try
            {
                var sports = await _context.Sports
                    .Select(s => new SportDTO
                    {
                        ID = s.ID,
                        Code = s.Code,
                        Name = s.Name,
                        RowVersion = s.RowVersion
                    })
                    .ToListAsync();

                if (!sports.Any())
                {
                    return NotFound(new { message = "No sports found in the database." });
                }

                return sports;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving sports.", details = ex.Message });
            }
        }

        // GET: api/Sports/inc
        // Includes the sports Atheletes
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<SportDTO>>> GetSportsWithAthletes()
        {
            try
            {
                var sports = await _context.Sports
                    .Include(s => s.Athletes)
                    .Select(s => new SportDTO
                    {
                        ID = s.ID,
                        Code = s.Code,
                        Name = s.Name,
                        RowVersion = s.RowVersion,
                        Athletes = s.Athletes != null ? s.Athletes.Select(a => new AthleteDTO
                        {
                            ID = a.ID,
                            FirstName = a.FirstName,
                            MiddleName = a.MiddleName,
                            LastName = a.LastName,
                            AthleteCode = a.AthleteCode,
                            DOB = a.DOB,
                            Height = a.Height,
                            Weight = a.Weight,
                            Gender = a.Gender,
                            Affiliation = a.Affiliation,
                            ContingentID = a.ContingentID,
                            Contingent = a.Contingent != null ? new ContingentDTO
                            {
                                ID = a.Contingent.ID,
                                Code = a.Contingent.Code,
                                Name = a.Contingent.Name
                            } : null,
                            SportID = a.SportID
                        }).ToList() : null
                    })
                    .ToListAsync();

                if (!sports.Any())
                {
                    return NotFound(new { message = "No sports with athletes found in the database." });
                }

                return sports;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving sports with athletes.", details = ex.Message });
            }
        }


        // GET: api/Sports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SportDTO>> GetSport(int id)
        {
            try
            {
                var sport = await _context.Sports
                    .Where(s => s.ID == id)
                    .Select(s => new SportDTO
                    {
                        ID = s.ID,
                        Code = s.Code,
                        Name = s.Name,
                        RowVersion = s.RowVersion
                    })
                    .FirstOrDefaultAsync();

                if (sport == null)
                {
                    return NotFound(new { message = "Sport not found." });
                }

                return sport;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving the sport.", details = ex.Message });
            }
        }





        // GET: api/Sports/inc/5
        // Includes the sports Atheletes
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<SportDTO>> GetSportWithAthletes(int id)
        {
            try
            {
                var sport = await _context.Sports
                    .Where(s => s.ID == id)
                    .Include(s => s.Athletes)
                    .Select(s => new SportDTO
                    {
                        ID = s.ID,
                        Code = s.Code,
                        Name = s.Name,
                        RowVersion = s.RowVersion,
                        Athletes = s.Athletes != null ? s.Athletes.Select(a => new AthleteDTO
                        {
                            ID = a.ID,
                            FirstName = a.FirstName,
                            MiddleName = a.MiddleName,
                            LastName = a.LastName,
                            AthleteCode = a.AthleteCode,
                            DOB = a.DOB,
                            Height = a.Height,
                            Weight = a.Weight,
                            Gender = a.Gender,
                            Affiliation = a.Affiliation,
                            ContingentID = a.ContingentID,
                            Contingent = a.Contingent != null ? new ContingentDTO
                            {
                                ID = a.Contingent.ID,
                                Code = a.Contingent.Code,
                                Name = a.Contingent.Name
                            } : null,
                            SportID = a.SportID
                        }).ToList() : null
                    })
                    .FirstOrDefaultAsync();

                if (sport == null)
                {
                    return NotFound(new { message = "Sport not found." });
                }

                return sport;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving the sport with athletes.", details = ex.Message });
            }
        }



        // // PUT: api/Sports/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutSport(int id, Sport sport)
        // {
        //     if (id != sport.ID)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(sport).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!SportExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Sports
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Sport>> PostSport(Sport sport)
        // {
        //     _context.Sports.Add(sport);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetSport", new { id = sport.ID }, sport);
        // }

        // // DELETE: api/Sports/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteSport(int id)
        // {
        //     var sport = await _context.Sports.FindAsync(id);
        //     if (sport == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Sports.Remove(sport);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool SportExists(int id)
        {
            return _context.Sports.Any(e => e.ID == id);
        }
    }
}
