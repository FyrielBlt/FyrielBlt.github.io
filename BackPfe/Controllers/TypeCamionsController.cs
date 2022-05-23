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
    public class TypeCamionsController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public TypeCamionsController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/TypeCamions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeCamion>>> GetTypeCamion()
        {
            return await _context.TypeCamion.ToListAsync();
        }

        // GET: api/TypeCamions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeCamion>> GetTypeCamion(int id)
        {
            var typeCamion = await _context.TypeCamion.FindAsync(id);

            if (typeCamion == null)
            {
                return NotFound();
            }

            return typeCamion;
        }

        // PUT: api/TypeCamions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeCamion(int id, TypeCamion typeCamion)
        {
            if (id != typeCamion.IdType)
            {
                return BadRequest();
            }

            _context.Entry(typeCamion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeCamionExists(id))
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

        // POST: api/TypeCamions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TypeCamion>> PostTypeCamion(TypeCamion typeCamion)
        {
            _context.TypeCamion.Add(typeCamion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeCamion", new { id = typeCamion.IdType }, typeCamion);
        }

        // DELETE: api/TypeCamions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeCamion>> DeleteTypeCamion(int id)
        {
            var typeCamion = await _context.TypeCamion.FindAsync(id);
            if (typeCamion == null)
            {
                return NotFound();
            }

            _context.TypeCamion.Remove(typeCamion);
            await _context.SaveChangesAsync();

            return typeCamion;
        }

        private bool TypeCamionExists(int id)
        {
            return _context.TypeCamion.Any(e => e.IdType == id);
        }
    }
}
