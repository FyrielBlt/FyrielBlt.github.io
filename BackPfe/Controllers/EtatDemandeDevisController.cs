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
    public class EtatDemandeDevisController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public EtatDemandeDevisController(BasePfeContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EtatDemandeDevis>>> GetEtatDemandeDevis()
        {
            return await _context.EtatDemandeDevis.ToListAsync();
        }
        // GET: api/EtatDemandeDevis
        [HttpGet("EtatDemandeDevis")]
        public async Task<ActionResult<EtatDemandeDevis>> GetEtatDemandeDevis([FromQuery] string etat)
        {
            EtatDemandeDevis etats = new EtatDemandeDevis();
            if (etat == "Accepte")
            {
                 etats =  _context.EtatDemandeDevis.Where(t => t.Etat == "Accepté").First();
            }

            if (etat == "Encours")
            {
                etats =  _context.EtatDemandeDevis.Where(t => t.Etat == "En cours de traitement").First();
            }
            if (etat == "Refuse")
            {
                etats =  _context.EtatDemandeDevis.Where(t => t.Etat == "Refusé").First();
            }



            return etats;
        }
    
        // GET: api/EtatDemandeDevis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EtatDemandeDevis>> GetEtatDemandeDevis(int id)
        {
            var etatDemandeDevis = await _context.EtatDemandeDevis.FindAsync(id);

            if (etatDemandeDevis == null)
            {
                return NotFound();
            }

            return etatDemandeDevis;
        }

        // PUT: api/EtatDemandeDevis/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtatDemandeDevis(int id, EtatDemandeDevis etatDemandeDevis)
        {
            if (id != etatDemandeDevis.IdEtat)
            {
                return BadRequest();
            }

            _context.Entry(etatDemandeDevis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtatDemandeDevisExists(id))
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

        // POST: api/EtatDemandeDevis
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EtatDemandeDevis>> PostEtatDemandeDevis(EtatDemandeDevis etatDemandeDevis)
        {
            _context.EtatDemandeDevis.Add(etatDemandeDevis);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEtatDemandeDevis", new { id = etatDemandeDevis.IdEtat }, etatDemandeDevis);
        }

        // DELETE: api/EtatDemandeDevis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EtatDemandeDevis>> DeleteEtatDemandeDevis(int id)
        {
            var etatDemandeDevis = await _context.EtatDemandeDevis.FindAsync(id);
            if (etatDemandeDevis == null)
            {
                return NotFound();
            }

            _context.EtatDemandeDevis.Remove(etatDemandeDevis);
            await _context.SaveChangesAsync();

            return etatDemandeDevis;
        }

        private bool EtatDemandeDevisExists(int id)
        {
            return _context.EtatDemandeDevis.Any(e => e.IdEtat == id);
        }
    }
}
