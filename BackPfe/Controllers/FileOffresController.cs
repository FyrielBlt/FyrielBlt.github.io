using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using BackPfe.Upload;
using Microsoft.AspNetCore.Hosting;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileOffresController : ControllerBase
    {
        private readonly BasePfeContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FileOffresController(BasePfeContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hostEnvironment = hosting;
        }

        // GET: api/FileOffres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileOffre>>> GetFileOffre()
        {
            return await _context.FileOffre.ToListAsync();
        }

        // GET: api/FileOffres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileOffre>> GetFileOffre(int id)
        {
            var fileOffre = await _context.FileOffre.FindAsync(id);

            if (fileOffre == null)
            {
                return NotFound();
            }
            fileOffre.SrcOffreFile = String.Format("{0}://{1}{2}/File/TransporteurFiles/OffreFiles/{3}", Request.Scheme, Request.Host,
                Request.PathBase, fileOffre.NomFile);
            return fileOffre;
        }

        // PUT: api/FileOffres/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileOffre(int id, FileOffre fileOffre)
        {
            if (id != fileOffre.IdFile)
            {
                return BadRequest();
            }

            _context.Entry(fileOffre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileOffreExists(id))
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

        // POST: api/FileOffres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<List<FileOffre>>> PostFileOffre([FromForm] FileOffre fileOffre)
        {
           
                fileOffre.NomFile = UploadFile.UploadImage(fileOffre.ImageFile, _hostEnvironment,
                 "File/TransporteurFiles/OffreFiles");
            _context.FileOffre.Add(fileOffre);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFileOffre", new
            {
                IdFile = fileOffre.IdFile,
                NomFile = fileOffre.NomFile,
                IdOffre = fileOffre.IdOffre,
                SrcOffreFile = String.Format("{0}://{1}{2}/File/TransporteurFiles/OffreFiles/{3}", Request.Scheme, Request.Host,
                Request.PathBase, fileOffre.NomFile),

            });
            

        }

        // DELETE: api/FileOffres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FileOffre>> DeleteFileOffre(int id)
        {
            var fileOffre = await _context.FileOffre.FindAsync(id);
            if (fileOffre == null)
            {
                return NotFound();
            }

            _context.FileOffre.Remove(fileOffre);
            await _context.SaveChangesAsync();

            return fileOffre;
        }

        private bool FileOffreExists(int id)
        {
            return _context.FileOffre.Any(e => e.IdFile == id);
        }
    }
}
