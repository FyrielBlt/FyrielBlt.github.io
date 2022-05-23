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
    public class TransporteursController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public TransporteursController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Transporteurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transporteur>>> GetTransporteur([FromQuery] Paginations pagination,
            [FromQuery] string name)
        {

            var queryable = _context.Transporteur
             .Select(x => new Transporteur()
             {
                 IdTransporteur = x.IdTransporteur,
                 IdUser = x.IdUser,
                 //x.IdUserNavigation.ImageSrc = string.Format("{0}://{1}{2}/imageFactureT/{3}", Request.Scheme, Request.Host, Request.PathBase, x.IdUserNavigation.Image,
                 IdUserNavigation = x.IdUserNavigation,
                 ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, x.IdUserNavigation.Image)
             })
                 .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                queryable = queryable.Where(s => s.IdUserNavigation.Nom.Contains(name) ||
                s.IdUserNavigation.Prenom.Contains(name) || s.IdUserNavigation.Email.Contains(name));
            }

            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<Transporteur> transporteurs = await queryable.Paginate(pagination).ToListAsync();

            return transporteurs;
            // return await _context.Transporteur.ToListAsync();
        }
        [HttpGet("{id}/iduser")]
        public async Task<ActionResult<Transporteur>> GetTransporteursbyiduser(int id)
        {
            var transporteur = _context.Transporteur.Where(x => x.IdUser == id)
                .Include(t => t.IdUserNavigation)

                 .First();

            if (transporteur == null)
            {
                return NotFound();
            }

            return transporteur;
        }
        // GET: api/Transporteurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transporteur>> GetTransporteur(int id)
        {
          /*  var transporteur = _context.Transporteur.Where(el => el.IdTransporteur == id)
                .Include(el => el.IdUserNavigation)
                .First(); ;*/
            var queryable = _context.Transporteur
           .Select(x => new Transporteur()
           {
               IdTransporteur = x.IdTransporteur,
               IdUser = x.IdUser,
               IdUserNavigation = x.IdUserNavigation,
               ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, x.IdUserNavigation.Image)
           })
               .AsQueryable();
            if (queryable == null)
            {
                return NotFound();
            }
             Transporteur transporteurs =  queryable.First();


            return transporteurs;
        }

        // PUT: api/Transporteurs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransporteur(int id, Transporteur transporteur)
        {
            if (id != transporteur.IdTransporteur)
            {
                return BadRequest();
            }

            _context.Entry(transporteur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransporteurExists(id))
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

        // POST: api/Transporteurs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Transporteur>> PostTransporteur(Transporteur transporteur)
        {
            _context.Transporteur.Add(transporteur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransporteur", new { id = transporteur.IdTransporteur }, transporteur);
        }

        // DELETE: api/Transporteurs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transporteur>> DeleteTransporteur(int id)
        {
            var transporteur = await _context.Transporteur.FindAsync(id);
            if (transporteur == null)
            {
                return NotFound();
            }

            _context.Transporteur.Remove(transporteur);
            await _context.SaveChangesAsync();

            return transporteur;
        }

        private bool TransporteurExists(int id)
        {
            return _context.Transporteur.Any(e => e.IdTransporteur == id);
        }
    }
}

