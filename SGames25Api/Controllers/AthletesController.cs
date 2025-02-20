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
        public async Task<ActionResult<AthleteDTO>> GetAthlete(int id)
        {


            var athleteDTO = await _context.Athletes
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
             }).FirstOrDefaultAsync(a => a.ID == id);

            if (athleteDTO != null)
            {
                return athleteDTO;
            }
            else
            {
                return NotFound(new { message = "Error: No Athelete records found in the database." });
            }
        }

        // PUT: api/Athletes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAthlete(int id, AthleteDTO athleteDTO)
        {
            if (id != athleteDTO.ID)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var athlete = await _context.Athletes.FindAsync(id);
            if (athlete == null)
            {
                return NotFound(new { message = "Athlete not found." });
            }

            // Update properties
            athlete.FirstName = athleteDTO.FirstName;
            athlete.MiddleName = athleteDTO.MiddleName;
            athlete.LastName = athleteDTO.LastName;
            athlete.AthleteCode = athleteDTO.AthleteCode;
            athlete.DOB = athleteDTO.DOB;
            athlete.Height = athleteDTO.Height;
            athlete.Weight = athleteDTO.Weight;
            athlete.Gender = athleteDTO.Gender;
            athlete.Affiliation = athleteDTO.Affiliation;
            athlete.ContingentID = athleteDTO.ContingentID;
            athlete.SportID = athleteDTO.SportID;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Athletes.Any(e => e.ID == id))
                {
                    return Conflict(new { message = "Concurrency Error: Athlete has been removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Athlete has been updated by another user. Please reload the data and try again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Athlete data (Athlete Code or another unique constraint)." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists, contact support." });
                }
            }
        }

        // POST: api/Athletes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<AthleteDTO>> PostAthlete(AthleteDTO athleteDTO)
        {
            if (athleteDTO == null)
            {
                return BadRequest(new { message = "Invalid athlete data." });
            }

            var athlete = new Athlete
            {
                FirstName = athleteDTO.FirstName,
                MiddleName = athleteDTO.MiddleName,
                LastName = athleteDTO.LastName,
                AthleteCode = athleteDTO.AthleteCode,
                DOB = athleteDTO.DOB,
                Height = athleteDTO.Height,
                Weight = athleteDTO.Weight,
                Gender = athleteDTO.Gender,
                Affiliation = athleteDTO.Affiliation,
                ContingentID = athleteDTO.ContingentID,
                SportID = athleteDTO.SportID
            };

            try
            {
                _context.Athletes.Add(athlete);
                await _context.SaveChangesAsync();

                var createdDTO = new AthleteDTO
                {
                    ID = athlete.ID,
                    FirstName = athlete.FirstName,
                    MiddleName = athlete.MiddleName,
                    LastName = athlete.LastName,
                    AthleteCode = athlete.AthleteCode,
                    DOB = athlete.DOB,
                    Height = athlete.Height,
                    Weight = athlete.Weight,
                    Gender = athlete.Gender,
                    Affiliation = athlete.Affiliation,
                    ContingentID = athlete.ContingentID,
                    SportID = athlete.SportID,
                    RowVersion = athlete.RowVersion
                };

                return CreatedAtAction(nameof(GetAthletes), new { id = athlete.ID }, createdDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Athlete Code detected." });
                }
                else
                {
                    return BadRequest(new { message = "Database error: Unable to save changes. Please try again later or contact support." });
                }
            }
        }

        // DELETE: api/Athletes/5

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAthlete(int id)
        {
            var athlete = await _context.Athletes.FindAsync(id);
            if (athlete == null)
            {
                return NotFound(new { message = "Athlete not found." });
            }

            try
            {
                _context.Athletes.Remove(athlete);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Concurrency Error: Athlete has already been removed by another user." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Database error: Unable to delete athlete. Ensure there are no dependencies preventing deletion." });
            }
        }

        [HttpGet("BySport/{id}")]
        public async Task<ActionResult<IEnumerable<AthleteDTO>>> GetAthletesBySport(int id)
        {
            try
            {
                var athletes = await _context.Athletes
                    .Where(a => a.SportID == id)
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

                if (!athletes.Any())
                {
                    return NotFound(new { message = "No athletes found for the specified sport." });
                }

                return athletes;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving athletes by sport.", details = ex.Message });
            }
        }


        [HttpGet("ByContingent/{id}")]
        public async Task<ActionResult<IEnumerable<AthleteDTO>>> GetAthletesByContingent(int id)
        {
            try
            {
                var athletes = await _context.Athletes
                    .Where(a => a.ContingentID == id)
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

                if (!athletes.Any())
                {
                    return NotFound(new { message = "No athletes found for the specified contingent." });
                }

                return athletes;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving athletes by contingent.", details = ex.Message });
            }
        }


        [HttpGet("ByFilter")]
        public async Task<ActionResult<IEnumerable<AthleteDTO>>> GetAthletesByFilter(int? ContingentID, int? SportID)
        {
            try
            {
                var query = _context.Athletes
                    .Include(c => c.Contingent)
                    .Include(s => s.Sport)
                    .AsQueryable();

                if (ContingentID.HasValue)
                {
                    query = query.Where(a => a.ContingentID == ContingentID.Value);
                }

                if (SportID.HasValue)
                {
                    query = query.Where(a => a.SportID == SportID.Value);
                }

                var athletes = await query
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

                if (!athletes.Any())
                {
                    return NotFound(new { message = "No athletes found matching the provided criteria." });
                }

                return athletes;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while retrieving athletes based on the filter.", details = ex.Message });
            }
        }




        private bool AthleteExists(int id)
        {
            return _context.Athletes.Any(e => e.ID == id);
        }
    }
}
