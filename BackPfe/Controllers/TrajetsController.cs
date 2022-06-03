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
    public class TrajetsController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public TrajetsController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Trajets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trajet>>> GetTrajet([FromQuery] Paginations pagination, [FromQuery] string ville1, [FromQuery] string ville2, [FromQuery] string date, [FromQuery] string today)
        {

            var queryable = _context.Trajet.Include(el => el.IdVille1Navigation)
                  .Include(el => el.IdVille2Navigation)
                  .Include(el => el.IdCamionNavigation.IdtransporteurNavigation)
                  .Include(el => el.IdCamionNavigation.IdtransporteurNavigation.IdUserNavigation)
                  .OrderByDescending(s => (s.Date).ToString())
                  .AsQueryable();
            if (!string.IsNullOrEmpty(date))
            {
                queryable = queryable.Where(s => (s.Date).ToString().Contains(date));
            }
            if (!string.IsNullOrEmpty(ville1) || !string.IsNullOrEmpty(ville2))
            {

                if (!string.IsNullOrEmpty(ville1) && !string.IsNullOrEmpty(ville2))
                {
                    foreach (Trajet trajet in queryable)
                    {
                        trajet.IdCamionNavigation.IdtransporteurNavigation.ImageSrc =
                             String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, trajet.IdCamionNavigation.IdtransporteurNavigation.IdUserNavigation.Image);
                    }
                    queryable = queryable.Where(el => el.IdVille1Navigation.NomVille.Contains(ville1) && el.IdVille2Navigation.NomVille.Contains(ville2));
                    // queryable = queryable.Where(el => el.IdVille1Navigation.NomVille.Contains(ville1) || el.IdVille2Navigation.NomVille.Contains(ville2));

                    //ajout nombre de page
                    await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
                    //element par page
                    List<Trajet> trajets = await queryable.Paginate(pagination).ToListAsync();

                    return trajets;
                }
                else
                {
                    foreach (Trajet trajet in queryable)
                    {
                        trajet.IdCamionNavigation.IdtransporteurNavigation.ImageSrc =
                             String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, trajet.IdCamionNavigation.IdtransporteurNavigation.IdUserNavigation.Image);
                    }
                    queryable = queryable.Where(el => el.IdVille1Navigation.NomVille.Contains(ville1) || el.IdVille2Navigation.NomVille.Contains(ville2));

                    //ajout nombre de page
                    await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
                    //element par page
                    List<Trajet> trajets = await queryable.Paginate(pagination).ToListAsync();

                    return trajets;
                }

            }
            else
            {
                foreach (Trajet trajet in queryable)
                {
                    trajet.IdCamionNavigation.IdtransporteurNavigation.ImageSrc =
                         String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, trajet.IdCamionNavigation.IdtransporteurNavigation.IdUserNavigation.Image);
                }
                //queryable = queryable.Where(el => el.IdVille1Navigation.NomVille.Contains(ville1) || el.IdVille2Navigation.NomVille.Contains(ville2));

                //ajout nombre de page
                await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
                //element par page
                List<Trajet> trajets = await queryable.Paginate(pagination).ToListAsync();

                return trajets;
            }

            // return await _context.Trajet.ToListAsync();
        }

        //get trajet de ce camion
        [HttpGet("{id}/camion")]
        public async Task<ActionResult<IEnumerable<Trajet>>> GetTrajetbycamion([FromQuery] Paginations pagination,
           [FromQuery] string depart,
              [FromQuery] string arrive,
               [FromQuery] string date , 
               

               int id)
        {
            // recherche pour demande de devis
           

                  var trajet = _context.Trajet.Where(t=>t.IdCamion==id)
                .Include(t=>t.IdCamionNavigation).ThenInclude(t=>t.IdchauffeurNavigation)
                  .Include(el => el.IdVille1Navigation)
                 .Include(el => el.IdVille2Navigation)
                 .OrderByDescending(t => t.IdTrajet)
                 .AsQueryable();
            if (!string.IsNullOrEmpty(depart))
            {
                trajet = trajet.Where(s => s.IdVille1Navigation.NomVille.Contains(depart) 
                );
            }
           
            if (!string.IsNullOrEmpty(arrive))
            {
                trajet = trajet.Where(s => s.IdVille2Navigation.NomVille.Contains(arrive)
             
               );
            }
            if (!string.IsNullOrEmpty(date))
            {

                trajet = trajet.Where(s => s.Date.ToString().Contains(date)
                );
            }

            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(trajet, pagination.QuantityPage);
                    //element par page
                    List<Trajet> trajets = await trajet.Paginate(pagination).ToListAsync();

                    return trajets;

               
            
            // return await _context.Trajet.ToListAsync();
        }
            [HttpGet("{id}/idtransporteur")]
        public async Task<ActionResult<IEnumerable<Trajet>>> GetTrajetbyidtransporteur(
          [FromQuery] Paginations pagination,
          [FromQuery] string depart,
          [FromQuery] string arrive,
          [FromQuery] DateTime date,
          [FromQuery] string date2,
          [FromQuery] string search,

              int id)
        {
            // recherche pour demande de devis
            var trajet = _context.Trajet.Where(t => t.IdCamionNavigation.IdtransporteurNavigation.IdTransporteur== id)
                .Where(t=>t.Date>=date)
            .Include(t => t.IdCamionNavigation).ThenInclude(t => t.IdchauffeurNavigation).ThenInclude(t=>t.IduserNavigation)
            .Include(t=>t.IdCamionNavigation).ThenInclude(t=>t.IdtypeNavigation)
            .Include(el => el.IdVille1Navigation)
            .Include(el => el.IdVille2Navigation)
            .OrderByDescending(t => t.IdTrajet)
            .AsQueryable();
            if (!string.IsNullOrEmpty(depart))
            {
                trajet = trajet.Where(s => s.IdVille1Navigation.NomVille.Contains(depart)
                );
            }
            if (!string.IsNullOrEmpty(arrive))
            {
                trajet = trajet.Where(s => s.IdVille2Navigation.NomVille.Contains(arrive)



               );
            }
            if (!string.IsNullOrEmpty(search))
            {
                trajet = trajet.Where(s => s.IdCamionNavigation.Codevehicule.Contains(search)
                );
            }
            if (date2!=null)
            {

                trajet = trajet.Where(s => s.Date.ToString().Contains(date2)

                );
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(trajet, pagination.QuantityPage);
            //element par page
            List<Trajet> trajets = await trajet.Paginate(pagination).ToListAsync();

            return trajets;



            // return await _context.Trajet.ToListAsync();
        }

        // GET: api/Trajets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trajet>> GetTrajet(int id)
        {
            var trajet = await _context.Trajet.FindAsync(id);

            if (trajet == null)
            {
                return NotFound();
            }

            return trajet;
        }
        [HttpGet("{id}/transporteur")]
        public async Task<ActionResult<IEnumerable<Trajet>>> GetTrajetbyidcmion(int id)
        {
            var trajets = _context.Trajet.Where(t => t.IdCamionNavigation.Idtransporteur == id)
                .Include(t=>t.IdVille1Navigation).Include(t=>t.IdVille2Navigation)
                .OrderByDescending(t => t.IdTrajet)
                .ToList();

            if (trajets == null)
            {
                return NotFound();
            }

            return trajets;
        }

        // PUT: api/Trajets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrajet(int id, Trajet trajet)
        {
            if (id != trajet.IdTrajet)
            {
                return BadRequest();
            }

            _context.Entry(trajet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrajetExists(id))
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

        // POST: api/Trajets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Trajet>> PostTrajet(Trajet trajet)
        {
            _context.Trajet.Add(trajet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrajet", new { id = trajet.IdTrajet }, trajet);
        }

        // DELETE: api/Trajets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trajet>> DeleteTrajet(int id)
        {
            var trajet = await _context.Trajet.FindAsync(id);
            if (trajet == null)
            {
                return NotFound();
            }

            _context.Trajet.Remove(trajet);
            await _context.SaveChangesAsync();

            return trajet;
        }

        private bool TrajetExists(int id)
        {
            return _context.Trajet.Any(e => e.IdTrajet == id);
        }
    }
}
