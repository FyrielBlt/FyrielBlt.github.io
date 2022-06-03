using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using BackPfe.Paginate;
using BackPfe.Upload;
using Microsoft.AspNetCore.Hosting;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocietesController : ControllerBase
    {
        private readonly BasePfeContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SocietesController(BasePfeContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hostEnvironment = hosting;
        }



        // GET: api/Societes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Societe>>> GetSociete([FromQuery] Paginations pagination, [FromQuery] string societe)
        {
            var queryable = _context.Societe
                 .Select(x => new Societe()
                 {
                     IdSociete = x.IdSociete,
                     Nom = x.Nom,
                     Image = x.Image,
                     Description = x.Description,
                     Adress = x.Adress,
                     ImageSrc = String.Format("{0}://{1}{2}/File/TransporteurFiles/ImageSociete/{3}", Request.Scheme, Request.Host, Request.PathBase, x.Image)
                 })

             .AsQueryable();

            if (!string.IsNullOrEmpty(societe))
            {
                queryable = queryable.Where(s => s.Nom.Contains(societe));
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<Societe> societes = await queryable.Paginate(pagination).ToListAsync();

            return societes;
            //  return await _context.Societe.ToListAsync();
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
        public async Task<ActionResult<Societe>> PutSociete(int id, [FromForm] Societe societe)
        {


            if (id != societe.IdSociete)
            {
                return BadRequest();
            }
            var socie = await _context.Societe.FindAsync(id);
            if (societe.ImageFile == null)
            {

                socie.Nom = societe.Nom;
                socie.Adress = societe.Adress;
                socie.Description = societe.Description;


                _context.Entry(socie).State = EntityState.Modified;
                socie.ImageSrc = String.Format("{0}://{1}{2}/File/TransporteurFiles/ImageSociete/{3}", Request.Scheme, Request.Host, Request.PathBase, socie.Image);

            }
            else
            {
                UploadFile.DeleteImage(societe.Image, _hostEnvironment, "File/TransporteurFiles/ImageSociete");
                socie.Image = UploadFile.UploadImage(societe.ImageFile, _hostEnvironment, "File/TransporteurFiles/ImageSociete");
                socie.Nom = societe.Nom;
                socie.Adress = societe.Adress;
                socie.Description = societe.Description;
                _context.Entry(socie).State = EntityState.Modified;
                socie.ImageSrc = String.Format("{0}://{1}{2}/File/TransporteurFiles/ImageSociete/{3}", Request.Scheme, Request.Host, Request.PathBase, socie.Image);

            }



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



            if (socie == null)
            {
                return NotFound();
            }

            return socie;
        }

        // POST: api/Societes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Societe>> PostSociete([FromForm] Societe societe)
        {
            /*_context.Societe.Add(societe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSociete", new { id = societe.IdSociete }, societe);*/
            if (societe.ImageFile!= null)
            {
                societe.Image = UploadFile.UploadImage(societe.ImageFile, _hostEnvironment, "File/TransporteurFiles/ImageSociete");
            }
            _context.Societe.Add(societe);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSociete", new
            {
                IdSociete = societe.IdSociete,
                Nom = societe.Nom,
                Image = societe.Image,
                Description = societe.Description,
                Adress = societe.Adress,
                ImageSrc = String.Format("{0}://{1}{2}/File/TransporteurFiles/ImageSociete/{3}", Request.Scheme, Request.Host, Request.PathBase, societe.Image)
            });



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
