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
    public class AthletesController : ControllerBase
    {
        private readonly SummerGamesContext _context;

        public AthletesController(SummerGamesContext context)
        {
            _context = context;
        }

        // GET: api/Athletes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AthleteDTO>>> GetAthletes()
        {
            var athleteDTOs = await _context.Athletes
             .Include(c => c.Contingent)
             .Include(s => s.Sport)
             .Select(a => new AthleteDTO
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
                 SportID = a.SportID,
                 Sport = a.Sport != null ? new SportDTO
                 {
                     ID = a.Sport.ID,
                     Code = a.Sport.Code,
                     Name = a.Sport.Name
                 } : null,
                 RowVersion = a.RowVersion
             })
             .ToListAsync();

            if (athleteDTOs.Count() > 0)
            {
                return athleteDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Athelete records found in the database." });
            }


        }

       

        // GET: api/Athletes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Athlete>> GetAthlete(int id)
        {
            var athlete = await _context.Athletes.FindAsync(id);

            if (athlete == null)
            {
                return NotFound();
            }

            return athlete;
        }

        // PUT: api/Athletes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAthlete(int id, Athlete athlete)
        {
            if (id != athlete.ID)
            {
                return BadRequest();
            }

            _context.Entry(athlete).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AthleteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Athletes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Athlete>> PostAthlete(Athlete athlete)
        {
            _context.Athletes.Add(athlete);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAthlete", new { id = athlete.ID }, athlete);
        }

        // DELETE: api/Athletes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAthlete(int id)
        {
            var athlete = await _context.Athletes.FindAsync(id);
            if (athlete == null)
            {
                return NotFound();
            }

            _context.Athletes.Remove(athlete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AthleteExists(int id)
        {
            return _context.Athletes.Any(e => e.ID == id);
        }
    }
}
