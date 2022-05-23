using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtatDemandeLivraisonsController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public EtatDemandeLivraisonsController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/EtatDemandeLivraisons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EtatDemandeLivraison>>> GetEtatDemandeLivraison()
        {
            return await _context.EtatDemandeLivraison.ToListAsync();
        }

        // GET: api/EtatDemandeLivraisons/5
        [HttpGet("check")]
        public async Task<ActionResult<EtatDemandeLivraison>> GetEtatDemandeLivraison([FromQuery] string etat)
        {
            EtatDemandeLivraison etats = new EtatDemandeLivraison();
            if (etat == "Accepte")
            {
                etats = await _context.EtatDemandeLivraison.Where(t => t.EtatDemande == "Accepté").FirstAsync();
            }
            if (etat == "Encours")
            {
                etats = await _context.EtatDemandeLivraison.Where(t => t.EtatDemande == "En cours de traitement").FirstAsync();
            }
            if (etat == "Refuse")
            {
                etats = await _context.EtatDemandeLivraison.Where(t => t.EtatDemande == "Refusé").FirstAsync();
            }
            if (etat == "livre")
            {
                etats = await _context.EtatDemandeLivraison.Where(t => t.EtatDemande == "Livré").FirstAsync();
            }


            return etats;
        }

        // PUT: api/EtatDemandeLivraisons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtatDemandeLivraison(int id, EtatDemandeLivraison etatDemandeLivraison)
        {
            if (id != etatDemandeLivraison.IdEtatDemande)
            {
                return BadRequest();
            }

            _context.Entry(etatDemandeLivraison).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtatDemandeLivraisonExists(id))
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

        // POST: api/EtatDemandeLivraisons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EtatDemandeLivraison>> PostEtatDemandeLivraison(EtatDemandeLivraison etatDemandeLivraison)
        {
            _context.EtatDemandeLivraison.Add(etatDemandeLivraison);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEtatDemandeLivraison", new { id = etatDemandeLivraison.IdEtatDemande }, etatDemandeLivraison);
        }

        // DELETE: api/EtatDemandeLivraisons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EtatDemandeLivraison>> DeleteEtatDemandeLivraison(int id)
        {
            var etatDemandeLivraison = await _context.EtatDemandeLivraison.FindAsync(id);
            if (etatDemandeLivraison == null)
            {
                return NotFound();
            }

            _context.EtatDemandeLivraison.Remove(etatDemandeLivraison);
            await _context.SaveChangesAsync();

            return etatDemandeLivraison;
        }

        private bool EtatDemandeLivraisonExists(int id)
        {
            return _context.EtatDemandeLivraison.Any(e => e.IdEtatDemande == id);
        }
    }
}
