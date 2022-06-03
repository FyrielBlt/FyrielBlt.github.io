using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using BackPfe.Paginate;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamionsController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public CamionsController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Camions 
        // Get all camions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camion>>> GetCamions()
        {
                List<Camion> camions = _context.Camion.Include(t=>t.IdtransporteurNavigation)
                .Include(t=>t.Trajet).ThenInclude(t=>t.IdVille1Navigation)
                .ToList();
                return camions;
        }

        // GET: api/Camions/5
        //Get camions by idtransporteur
        [HttpGet("{idtransporteur}")]
        public async Task<ActionResult<IEnumerable<Camion>>> GetCamionsbyidtrabsporteur([FromQuery] Paginations pagination,
            [FromQuery] string search, [FromQuery] string type, int idtransporteur)
        {
           
                var camion = _context.Camion.Where(t => t.Idtransporteur == idtransporteur)
                  
                .Include(t => t.IdtransporteurNavigation)
                .Include(t => t.IdchauffeurNavigation).ThenInclude(t=>t.IduserNavigation)
                .Include(t=>t.Trajet).ThenInclude(t=>t.IdVille1Navigation).ThenInclude(t => t.TrajetIdVille2Navigation)
                .Include(t=>t.IdtypeNavigation)
                                 .OrderByDescending(t => t.Idcamion)

                .AsQueryable();
            if (!string.IsNullOrEmpty(type))
            {
                camion = camion.Where(s => s.Idtype.ToString().Contains(type)

                );
            }
            if (!string.IsNullOrEmpty(search))
            {
                camion = camion.Where(s => s.Codevehicule.Contains(search) ||
                s.IdchauffeurNavigation.IduserNavigation.Nom.Contains(search)


                );
            }

            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(camion, pagination.QuantityPage);
            //element par page
            List<Camion> camions = await camion.Paginate(pagination).ToListAsync();
            return camions;

            
        }
        
        [HttpGet("{idchauffeur}/chauffeur")]
        //get camion by idchauffeur
        public async Task<ActionResult<Camion>> GetCamionsbyidchauffeur(int idchauffeur)
        {
            Camion camion = await _context.Camion.Where(t => t.Idchauffeur == idchauffeur)
           .Include(t => t.IdtransporteurNavigation).FirstAsync();
            return camion;
        }
        [HttpGet("{id}/camion")]
        // get camion by id
        public async Task<ActionResult<IEnumerable<Camion>>> GetCamions([FromQuery] Paginations pagination, [FromQuery] string search, int idtransporteur,int id)
        {
            var camion =  _context.Camion.Where(t => t.Idcamion == id)
               .Include(t => t.IdtransporteurNavigation)
               .Include(t => t.IdchauffeurNavigation)
               .Include(t => t.Trajet).ThenInclude(t => t.IdVille2Navigation)
               .Include(t => t.Trajet).ThenInclude(t => t.IdVille1Navigation)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                camion = camion.Where(s => s.Codevehicule.Contains(search) ||
                s.IdchauffeurNavigation.IduserNavigation.Nom.Contains(search) 
                
                );
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(camion, pagination.QuantityPage);
            //element par page
            List<Camion> camions = await camion.Paginate(pagination).ToListAsync();
            return camions;
        }
        [HttpGet("{id}/camion/codevehicule")]
        //get camion by codevehicule
        public async Task<ActionResult<Camion>> GetCamionsbycodevehicule(string id)
        {
            Camion camions = await _context.Camion.Where(t => t.Codevehicule == id)
               .Include(t => t.IdtransporteurNavigation)
               .Include(t => t.IdchauffeurNavigation)
               .FirstAsync();

            if (camions == null)
            {
                return NotFound();
            }

            return camions;
        }

        // PUT: api/Camions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamions(int id, Camion camions)
        {
            List<Camion> test = _context.Camion.Where(t => t.Codevehicule == camions.Codevehicule)
                .Where(t => t.Idcamion!= camions.Idcamion)
                .ToList();
            if (test.Count == 0)
            {


                if (id != camions.Idcamion)
                {
                    return BadRequest();
                }

                _context.Entry(camions).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamionsExists(id))
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
            else return NotFound("CODEVEHICULE EXISTE");
        }

        // POST: api/Camions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Camion>> PostCamions(Camion camions)
        {
            List<Camion> test = _context.Camion.Where(t => t.Codevehicule == camions.Codevehicule)
                .ToList();
            if (test.Count == 0)
            {
                _context.Camion.Add(camions);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCamions", new { id = camions.Idcamion }, camions);
            }
            else
            {
                return NotFound("codevehicule existe");
            }
        }

        // DELETE: api/Camions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Camion>> DeleteCamions(int id)
        {
            var camions = await _context.Camion.FindAsync(id);
            if (camions == null)
            {
                return NotFound();
            }

            _context.Camion.Remove(camions);
            await _context.SaveChangesAsync();

            return camions;
        }

        private bool CamionsExists(int id)
        {
            return _context.Camion.Any(e => e.Idcamion == id);
        }
    }
}
