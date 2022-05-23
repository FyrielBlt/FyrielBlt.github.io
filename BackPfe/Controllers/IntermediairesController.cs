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
    public class IntermediairesController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public IntermediairesController(BasePfeContext context)
        {
            _context = context;
        }
        // GET: api/Intermediaires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Intermediaire>>> GetIntermediaire([FromQuery] Paginations pagination, [FromQuery] int idRole)
        {
            /*  List<Intermediaire> intermediaires = _context.Intermediaire
                  .Include(t => t.IdUserNavigation)
                  .Include(t => t.IdRoleNavigation)
                  .ToList();*/
            // return intermediaires;
            /* var queryable = _context.Intermediaire
                 .Include(t => t.IdUserNavigation)
                 .Include(t => t.IdRoleNavigation)
                 .AsQueryable();*/
            var queryable = _context.Intermediaire
            .Select(x => new Intermediaire()
            {
                IdIntermediaire = x.IdIntermediaire,
                IdUser = x.IdUser,
                IdRole = x.IdRole,
                IdUserNavigation = x.IdUserNavigation,
                IdRoleNavigation = x.IdRoleNavigation,
                ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, x.IdUserNavigation.Image)
            })
                .AsQueryable();

            if (idRole != 0)
            {
                queryable = queryable.Where(hh => hh.IdRole == idRole);
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<Intermediaire> Intermediaires = await queryable.Paginate(pagination).ToListAsync();

            return Intermediaires;
        }

        // GET: api/Intermediaires/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Intermediaire>> GetIntermediaire(int id)
        {
            var intermediaire = _context.Intermediaire.Include(el => el.IdUserNavigation)
                .Include(el => el.IdRoleNavigation).Where(el => el.IdIntermediaire == id).First();

            if (intermediaire == null)
            {
                return NotFound();
            }
            intermediaire.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, intermediaire.IdUserNavigation.Image);

            return intermediaire;
        }

        // PUT: api/Intermediaires/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntermediaire(int id, Intermediaire intermediaire)
        {
            if (id != intermediaire.IdIntermediaire)
            {
                return BadRequest();
            }

            _context.Entry(intermediaire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntermediaireExists(id))
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

        // POST: api/Intermediaires
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Intermediaire>> PostIntermediaire(Intermediaire intermediaire)
        {
            _context.Intermediaire.Add(intermediaire);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntermediaire", new { id = intermediaire.IdIntermediaire }, intermediaire);
        }

        // DELETE: api/Intermediaires/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Intermediaire>> DeleteIntermediaire(int id)
        {
            var intermediaire = await _context.Intermediaire.FindAsync(id);
            if (intermediaire == null)
            {
                return NotFound();
            }

            _context.Intermediaire.Remove(intermediaire);
            await _context.SaveChangesAsync();

            return intermediaire;
        }

        private bool IntermediaireExists(int id)
        {
            return _context.Intermediaire.Any(e => e.IdIntermediaire == id);
        }
    }
}
