using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using Microsoft.AspNetCore.Hosting;
using BackPfe.Upload;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDemandeLivraisonsController : ControllerBase
    {
        private readonly BasePfeContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FileDemandeLivraisonsController(BasePfeContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hostEnvironment = hosting;
        }

        // GET: api/FileDemandeLivraisons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileDemandeLivraison>>> GetFileDemandeLivraison()
        {
            return await _context.FileDemandeLivraison.ToListAsync();
        }

        // GET: api/FileDemandeLivraisons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileDemandeLivraison>> GetFileDemandeLivraison(int id)
        {
            var fileDemandeLivraison = await _context.FileDemandeLivraison.FindAsync(id);

            if (fileDemandeLivraison == null)
            {
                return NotFound();
            }

            return fileDemandeLivraison;
        }

        // PUT: api/FileDemandeLivraisons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileDemandeLivraison(int id, FileDemandeLivraison fileDemandeLivraison)
        {
            if (id != fileDemandeLivraison.IdFile)
            {
                return BadRequest();
            }

            _context.Entry(fileDemandeLivraison).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileDemandeLivraisonExists(id))
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

        // POST: api/FileDemandeLivraisons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FileDemandeLivraison>> PostFileDemandeLivraison([FromForm] FileDemandeLivraison fileDemandeLivraison)
        {
            fileDemandeLivraison.NomFile = UploadFile.UploadImage(fileDemandeLivraison.File, _hostEnvironment, "File/Client/DemandeLivraison");
            _context.FileDemandeLivraison.Add(fileDemandeLivraison);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFileDemandeLivraison", new
            {
                IdFile = fileDemandeLivraison.IdFile,
                NomFile = fileDemandeLivraison.NomFile,
                IdDemande = fileDemandeLivraison.IdDemande,
                SrcFile = String.Format("{0}://{1}{2}/File/Client/DemandeLivraison/{3}", Request.Scheme, Request.Host, Request.PathBase, fileDemandeLivraison.NomFile),

            });
            
        }

        // DELETE: api/FileDemandeLivraisons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FileDemandeLivraison>> DeleteFileDemandeLivraison(int id)
        {
            var fileDemandeLivraison = await _context.FileDemandeLivraison.FindAsync(id);
            if (fileDemandeLivraison == null)
            {
                return NotFound();
            }

            _context.FileDemandeLivraison.Remove(fileDemandeLivraison);
            await _context.SaveChangesAsync();

            return fileDemandeLivraison;
        }

        private bool FileDemandeLivraisonExists(int id)
        {
            return _context.FileDemandeLivraison.Any(e => e.IdFile == id);
        }
    }
}
