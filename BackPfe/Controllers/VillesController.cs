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
    public class VillesController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public VillesController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Villes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ville>>> GetVille([FromQuery] Paginations pagination, [FromQuery] string ville)
        {
            var queryable = _context.Ville

              .AsQueryable();

            if (!string.IsNullOrEmpty(ville))
            {
                queryable = queryable.Where(s => s.NomVille.Contains(ville));
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<Ville> villes = await queryable.Paginate(pagination).ToListAsync();

            return villes;
            //return await _context.Ville.ToListAsync();
        }

        // GET: api/Villes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ville>> GetVille(int id)
        {
            var ville = await _context.Ville.FindAsync(id);

            if (ville == null)
            {
                return NotFound();
            }

            return ville;
        }
        [HttpGet("{id}/transporteur")]
        public async Task<ActionResult<IEnumerable<Ville>>> GetVilleshown(int id)
        {
            var itineraires = _context.Itineraire.Where(t => t.IdTransporteur == id).ToList();
            var villes = _context.Ville.AsQueryable();

                foreach(Itineraire i in itineraires)
            {
                villes = villes.Where(t => t.IdVille != i.IdVille);
            }
;           

            if (villes == null)
            {
                return NotFound();
            }

            return villes.ToList();
        }
       
    //villebyname
    [HttpGet("{ville}/ville")]

        public async Task<ActionResult<Ville>> GetVillebyname(string ville)
        {
            var villee = _context.Ville.Where(t => t.NomVille == ville).First();

            if (villee == null)
            {
                return NotFound();
            }

            return villee;
        }

        // PUT: api/Villes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVille(int id, Ville ville)
        {
            if (id != ville.IdVille)
            {
                return BadRequest();
            }

            _context.Entry(ville).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VilleExists(id))
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

        // POST: api/Villes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Ville>> PostVille(Ville ville)
        {
            _context.Ville.Add(ville);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVille", new { id = ville.IdVille }, ville);
        }

        // DELETE: api/Villes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ville>> DeleteVille(int id)
        {
            var ville = await _context.Ville.FindAsync(id);
            if (ville == null)
            {
                return NotFound();
            }

            _context.Ville.Remove(ville);
            await _context.SaveChangesAsync();

            return ville;
        }

        private bool VilleExists(int id)
        {
            return _context.Ville.Any(e => e.IdVille == id);
        }
    }
}
