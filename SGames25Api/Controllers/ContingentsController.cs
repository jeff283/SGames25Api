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
    public class ContingentsController : ControllerBase
    {
        private readonly SummerGamesContext _context;

        public ContingentsController(SummerGamesContext context)
        {
            _context = context;
        }

        // GET: api/Contingents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContingentDTO>>> GetContingents()
        {
            try
            {
                var contingents = await _context.Contingents
                    .Select(c => new ContingentDTO
                    {
                        ID = c.ID,
                        Code = c.Code,
                        Name = c.Name
                    })
                    .ToListAsync();

                if (!contingents.Any())
                {
                    return NotFound(new { message = "No contingents found in the database." });
                }

                return contingents;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving contingents.", details = ex.Message });
            }
        }

        // GET: api/Contingents/inc
        // (Include athletes)
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<ContingentDTO>>> GetContingentsWithAthletes()
        {
            try
            {
                var contingents = await _context.Contingents
                    .Include(c => c.Athletes)
                    .Select(c => new ContingentDTO
                    {
                        ID = c.ID,
                        Code = c.Code,
                        Name = c.Name,
                        Athletes = c.Athletes != null ? c.Athletes.Select(a => new AthleteDTO
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
                            SportID = a.SportID,
                            Sport = a.Sport != null ? new SportDTO
                            {
                                ID = a.Sport.ID,
                                Code = a.Sport.Code,
                                Name = a.Sport.Name
                            } : null
                        }).ToList() : null
                    })
                    .ToListAsync();

                if (!contingents.Any())
                {
                    return NotFound(new { message = "No contingents with athletes found in the database." });
                }

                return contingents;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving contingents with athletes.", details = ex.Message });
            }
        }


        // GET: api/Contingents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContingentDTO>> GetContingent(int id)
        {
            try
            {
                var contingent = await _context.Contingents
                    .Where(c => c.ID == id)
                    .Select(c => new ContingentDTO
                    {
                        ID = c.ID,
                        Code = c.Code,
                        Name = c.Name
                    })
                    .FirstOrDefaultAsync();

                if (contingent == null)
                {
                    return NotFound(new { message = "Contingent not found." });
                }

                return contingent;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving the contingent.", details = ex.Message });
            }
        }


        // GET: api/Contingents/inc/5
        // (Include athletes)
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<ContingentDTO>> GetContingentWithAthletes(int id)
        {
            try
            {
                var contingent = await _context.Contingents
                    .Where(c => c.ID == id)
                    .Include(c => c.Athletes)
                    .Select(c => new ContingentDTO
                    {
                        ID = c.ID,
                        Code = c.Code,
                        Name = c.Name,
                        Athletes = c.Athletes != null ? c.Athletes.Select(a => new AthleteDTO
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
                            SportID = a.SportID,
                            Sport = a.Sport != null ? new SportDTO
                            {
                                ID = a.Sport.ID,
                                Code = a.Sport.Code,
                                Name = a.Sport.Name
                            } : null
                        }).ToList() : null
                    })
                    .FirstOrDefaultAsync();

                if (contingent == null)
                {
                    return NotFound(new { message = "Contingent not found." });
                }

                return contingent;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving the contingent with athletes.", details = ex.Message });
            }
        }





        // // PUT: api/Contingents/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutContingent(int id, Contingent contingent)
        // {
        //     if (id != contingent.ID)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(contingent).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ContingentExists(id))
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

        // // POST: api/Contingents
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Contingent>> PostContingent(Contingent contingent)
        // {
        //     _context.Contingents.Add(contingent);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetContingent", new { id = contingent.ID }, contingent);
        // }

        // // DELETE: api/Contingents/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteContingent(int id)
        // {
        //     var contingent = await _context.Contingents.FindAsync(id);
        //     if (contingent == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Contingents.Remove(contingent);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ContingentExists(int id)
        {
            return _context.Contingents.Any(e => e.ID == id);
        }
    }
}
