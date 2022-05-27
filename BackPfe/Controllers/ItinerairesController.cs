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
        public async Task<ActionResult<IEnumerable<Itineraire>>> GetItineraire()
        {
            return await _context.Itineraire.ToListAsync();
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
            var itineraire =  _context.Itineraire.Where(t => t.IdTransporteur == id).
                Include(t=>t.IdVilleNavigation).
                ToList()
           
               ;

            if (itineraire == null)
            {
                return NotFound();
            }

            return itineraire;
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
