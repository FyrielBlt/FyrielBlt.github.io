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
    public class SocietesController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public SocietesController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Societes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Societe>>> GetSociete()
        {
            return await _context.Societe.ToListAsync();
        }

        // GET: api/Societes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Societe>> GetSociete(int id)
        {
            var societe = await _context.Societe.FindAsync(id);

            if (societe == null)
            {
                return NotFound();
            }

            return societe;
        }

        // PUT: api/Societes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSociete(int id, Societe societe)
        {
            if (id != societe.IdSociete)
            {
                return BadRequest();
            }

            _context.Entry(societe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocieteExists(id))
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

        // POST: api/Societes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Societe>> PostSociete(Societe societe)
        {
            _context.Societe.Add(societe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSociete", new { id = societe.IdSociete }, societe);
        }

        // DELETE: api/Societes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Societe>> DeleteSociete(int id)
        {
            var societe = await _context.Societe.FindAsync(id);
            if (societe == null)
            {
                return NotFound();
            }

            _context.Societe.Remove(societe);
            await _context.SaveChangesAsync();

            return societe;
        }

        private bool SocieteExists(int id)
        {
            return _context.Societe.Any(e => e.IdSociete == id);
        }
    }
}
