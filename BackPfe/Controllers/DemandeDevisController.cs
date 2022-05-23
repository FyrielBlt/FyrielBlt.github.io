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
    public class DemandeDevisController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public DemandeDevisController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/DemandeDevis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeDevis>>> GetDemandeDevis([FromQuery] Paginations pagination)
        {

            var queryable= _context.DemandeDevis.Include(t => t.IdTransporteurNavigation)
                                             .AsQueryable();


            /*if (!string.IsNullOrEmpty(ville1)&& !string.IsNullOrEmpty(ville2))
            {
                var quer = _context.Camion.Include(el => el.Trajet)
                    .ThenInclude(el => el.IdVille2Navigation)
                    .Where(el => el.IdVille1Navigation.NomVille == ville1 && el.IdVille2Navigation.NomVille==ville2);
                foreach(DemandeDevis trajet in queryable)
                {
                    trajet.IdTransporteurNavigation.Camion
                }
                queryable = queryable.Where(s => s.IdTransporteurNavigation.Contains(permission));
            }*/
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<DemandeDevis> DemandeDevis = await queryable.Paginate(pagination).ToListAsync();

            return DemandeDevis;

            /*return _context.DemandeDevis.Include(t=>t.IdDemandeNavigation)
                                            .Include(t => t.IdIntermediaireNavigation)
                                            .Include(t => t.IdTransporteurNavigation)
                                            .ToList();*/
        }

        // GET: api/DemandeDevis/5
        //get list demande de devis par id demande
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DemandeDevis>>> GetDemandeDevis(int id)
        {
            return _context.DemandeDevis.Where(el => el.IdDemande == id).Include(t => t.IdIntermediaireNavigation)
                                            .Include(t => t.IdTransporteurNavigation)
                                            .ToList(); ;

            /*if (demandeDevis == null)
            {
                return NotFound();
            }

            return "rrr";*/
        }
        // les demande devise de ce transporteur qui sont non traité
        [HttpGet("{id}/transporteur")]
        public async Task<ActionResult<IEnumerable<DemandeDevis>>> GetDemandeDevisbyidtransporteur(
            [FromQuery] Paginations pagination, [FromQuery] string search,
            [FromQuery] string depart,
            [FromQuery] string arrive,
            [FromQuery] string date,
            [FromQuery] string today,

            int id)
        {
            var demandeDevis = _context.DemandeDevis.Where(t => t.IdTransporteur == id)

                .Where(t=>t.IdEtatNavigation.Etat== "Non traité")


                .Include(t => t.IdDemandeNavigation).ThenInclude(t => t.IdEtatdemandeNavigation)
                .Include(t=>t.IdEtatNavigation)
                .Include(t => t.IdDemandeNavigation).ThenInclude(t => t.FileDemandeLivraison)


                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                demandeDevis = demandeDevis.Where(s => s.IdDemandeNavigation.Description.Contains(search) 
                || s.IdDemandeNavigation.Adressdepart.Contains(search)
                || s.IdDemandeNavigation.Adressarrive.Contains(search)
                );
            }
            if (!string.IsNullOrEmpty(depart))
            {
                demandeDevis = demandeDevis.Where(s=>
                 s.IdDemandeNavigation.Adressdepart.Contains(depart)
              
                );
            }
            if (!string.IsNullOrEmpty(today))
            {
                demandeDevis = demandeDevis.Where(s =>
                 s.DateEnvoit.ToString().Contains(today)

                );
            }
            if (!string.IsNullOrEmpty(date))
            {
                demandeDevis = demandeDevis.Where(s =>
                 (s.IdDemandeNavigation.Date).ToString().Contains(date)

                );
            }
            if (!string.IsNullOrEmpty(arrive))
            {
                demandeDevis = demandeDevis.Where(s =>
                 s.IdDemandeNavigation.Adressarrive.Contains(arrive)

                );
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(demandeDevis, pagination.QuantityPage);
            //element par page
            List<DemandeDevis> demandes = await demandeDevis.Paginate(pagination).ToListAsync();

            if (demandeDevis == null)
            {
                return NotFound();
            }

            return demandes;
        }
        // les demande devise de ce transporteur qui sont traité ou e cours de traitement
        [HttpGet("{id}/traite")]

        public async Task<ActionResult<IEnumerable<DemandeDevis>>> GetDemandeDevisbyidtransporteur2(
               [FromQuery] Paginations pagination, [FromQuery] string search,
             [FromQuery] string depart,
              [FromQuery] string arrive,
               [FromQuery] string date,
                [FromQuery] string etat,
            int id)
        {
            var demandeDevis = _context.DemandeDevis.Where(t => t.IdTransporteur == id)
              .Where(t => t.IdEtatNavigation.IdEtat != 1002)
                .Include(t => t.IdDemandeNavigation).ThenInclude(t => t.IdEtatdemandeNavigation)
                .Include(t=> t.IdEtatNavigation)
                 .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                demandeDevis = demandeDevis.Where(s => s.IdDemandeNavigation.Description.Contains(search)
                || s.IdDemandeNavigation.Adressdepart.Contains(search)
                || s.IdDemandeNavigation.Adressarrive.Contains(search)
                );
            }
            if (!string.IsNullOrEmpty(depart))
            {
                demandeDevis = demandeDevis.Where(s =>
                 s.IdDemandeNavigation.Adressdepart.Contains(depart)

                );
            }
            if (!string.IsNullOrEmpty(etat))
            {
                if (etat == "Accepte")
                {
                    demandeDevis = demandeDevis.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Accepté"));
                }
                   
                if (etat == "Refuse")
                {
                    demandeDevis = demandeDevis.Where(s =>
                s.IdEtatNavigation.Etat.Contains("Refusé"));
                }
                    

                    
            }
            if (!string.IsNullOrEmpty(date))
            {
                demandeDevis = demandeDevis.Where(s =>
                 (s.IdDemandeNavigation.Date).ToString().Contains(date)

                );
            }
            if (!string.IsNullOrEmpty(arrive))
            {
                demandeDevis = demandeDevis.Where(s =>
                 s.IdDemandeNavigation.Adressarrive.Contains(arrive)

                );
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(demandeDevis, pagination.QuantityPage);
            //element par page
            List<DemandeDevis> demandes = await demandeDevis.Paginate(pagination).ToListAsync();

            if (demandeDevis == null)
            {
                return NotFound();
            }

            return demandes;
        }


        // PUT: api/DemandeDevis/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDemandeDevis(int id, DemandeDevis demandeDevis)
        {
            if (id != demandeDevis.IdDemandeDevis)
            {
                return BadRequest();
            }

            _context.Entry(demandeDevis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeDevisExists(id))
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

        // POST: api/DemandeDevis
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DemandeDevis>> PostDemandeDevis(DemandeDevis demandeDevis)
        {
            _context.DemandeDevis.Add(demandeDevis);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDemandeDevis", new { id = demandeDevis.IdDemandeDevis }, demandeDevis);
        }

        // DELETE: api/DemandeDevis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DemandeDevis>> DeleteDemandeDevis(int id)
        {
            var demandeDevis = await _context.DemandeDevis.FindAsync(id);
            if (demandeDevis == null)
            {
                return NotFound();
            }

            _context.DemandeDevis.Remove(demandeDevis);
            await _context.SaveChangesAsync();

            return demandeDevis;
        }

        private bool DemandeDevisExists(int id)
        {
            return _context.DemandeDevis.Any(e => e.IdDemandeDevis == id);
        }
    }
}
