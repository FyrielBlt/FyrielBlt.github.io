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
    public class ItinerairesController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public ItinerairesController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Itineraires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Itineraire>>> GetItineraire([FromQuery] Paginations pagination, [FromQuery] string ville1, [FromQuery] string name)
        {
            var queryable = _context.Itineraire

            .Include(t => t.IdTransporteurNavigation).ThenInclude(t => t.IdUserNavigation)
            .Include(t => t.IdVilleNavigation)

            .AsQueryable();
            foreach (Itineraire demande in queryable)
            {
                demande.IdTransporteurNavigation.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, demande.IdTransporteurNavigation.IdUserNavigation.Image);
            }
            if (!string.IsNullOrEmpty(ville1))
            {
                queryable = queryable.Where(s => s.IdVilleNavigation.NomVille.Contains(ville1));
            }
            if (!string.IsNullOrEmpty(name))
            {
                queryable = queryable.Where(s => s.IdTransporteurNavigation.IdUserNavigation.Nom.Contains(name) || s.IdTransporteurNavigation.IdUserNavigation.Prenom.Contains(name));
            }
            // return await _context.Itineraire.ToListAsync();
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<Itineraire> demandeLivraisons = await queryable.Paginate(pagination).ToListAsync();

            return demandeLivraisons;
        }

        // GET: api/Itineraires/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Itineraire>> GetItineraire(int id)
        {
            var itineraire = await _context.Itineraire.FindAsync(id);

            if (itineraire == null)
            {
                return NotFound();
            }

            return itineraire;
        }
        [HttpGet("{id}/transporteur")]
        public async Task<ActionResult<IEnumerable<Itineraire>>> GetItinerairebyidtransporteur( [FromQuery] Paginations pagination, int id)
        {
            var itineraire = _context.Itineraire.Where(t => t.IdTransporteur == id).
                Include(t => t.IdVilleNavigation).OrderByDescending(t=>t.IdItineraire).
                AsQueryable();
           
               ;

            if (itineraire == null)
            {
                return NotFound();
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(itineraire, pagination.QuantityPage);
            //element par page
            List<Itineraire> itineraires = await itineraire.Paginate(pagination).ToListAsync();
            return itineraires;
        }

        // PUT: api/Itineraires/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItineraire(int id, Itineraire itineraire)
        {
            if (id != itineraire.IdItineraire)
            {
                return BadRequest();
            }

            _context.Entry(itineraire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItineraireExists(id))
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

        // POST: api/Itineraires
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Itineraire>> PostItineraire(Itineraire itineraire)
        {
            _context.Itineraire.Add(itineraire);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItineraire", new { id = itineraire.IdItineraire }, itineraire);
        }

        // DELETE: api/Itineraires/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Itineraire>> DeleteItineraire(int id)
        {
            var itineraire = await _context.Itineraire.FindAsync(id);
            if (itineraire == null)
            {
                return NotFound();
            }

            _context.Itineraire.Remove(itineraire);
            await _context.SaveChangesAsync();

            return itineraire;
        }

        private bool ItineraireExists(int id)
        {
            return _context.Itineraire.Any(e => e.IdItineraire == id);
        }
    }
}
