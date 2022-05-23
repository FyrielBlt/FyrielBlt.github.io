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
    public class EtatOffresController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public EtatOffresController(BasePfeContext context)
        {
            _context = context;
        }
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<EtatOffre>>> GetEtatOffre()
        {

            return await _context.EtatOffre.ToListAsync();


        }
        // GET: api/EtatOffres
        [HttpGet("offre")]
        public async Task<ActionResult<EtatOffre>> GetEtatOffre([FromQuery] string offre)
        {
            EtatOffre offres = new EtatOffre();
            if (offre == "Accepte")
            {
                offres = await _context.EtatOffre.Where(t => t.Etat == "Accepté").FirstAsync();
            }

            if (offre == "Nontraite")
            {
                offres = await _context.EtatOffre.Where(t => t.Etat == "Non traité").FirstAsync();
            }



            return offres;
        }

        // GET: api/EtatOffres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EtatOffre>> GetEtatOffre(int id)
        {
            var etatOffre = await _context.EtatOffre.FindAsync(id);

            if (etatOffre == null)
            {
                return NotFound();
            }

            return etatOffre;
        }

        // PUT: api/EtatOffres/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtatOffre(int id, EtatOffre etatOffre)
        {
            if (id != etatOffre.IdEtat)
            {
                return BadRequest();
            }

            _context.Entry(etatOffre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtatOffreExists(id))
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

        // POST: api/EtatOffres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EtatOffre>> PostEtatOffre(EtatOffre etatOffre)
        {
            _context.EtatOffre.Add(etatOffre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEtatOffre", new { id = etatOffre.IdEtat }, etatOffre);
        }

        // DELETE: api/EtatOffres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EtatOffre>> DeleteEtatOffre(int id)
        {
            var etatOffre = await _context.EtatOffre.FindAsync(id);
            if (etatOffre == null)
            {
                return NotFound();
            }

            _context.EtatOffre.Remove(etatOffre);
            await _context.SaveChangesAsync();

            return etatOffre;
        }

        private bool EtatOffreExists(int id)
        {
            return _context.EtatOffre.Any(e => e.IdEtat == id);
        }
    }
}
