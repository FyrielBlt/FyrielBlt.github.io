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
namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeLivraisonsController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public DemandeLivraisonsController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/DemandeLivraisons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DemandeLivraison>>> GetDemandeLivraison([FromQuery] Paginations pagination, [FromQuery] string sortOrder, [FromQuery] string name, [FromQuery] string num, [FromQuery] string date, [FromQuery] int filter = 0, [FromQuery] int filter1 = 0)
        {
            var queryable = _context.DemandeLivraison
               .Include(t => t.DemandeDevis)
               .Include(t => t.Offre)
               .Include(t => t.FactureClient)
               .Include(t => t.IdclientNavigation)
               .ThenInclude(t => t.IduserNavigation)
               .Include(t => t.IdEtatdemandeNavigation)
                .Where(hh => hh.IdEtatdemandeNavigation.EtatDemande != "Livré" && hh.IdEtatdemandeNavigation.EtatDemande != "Achevé")
               .OrderBy(t => t.IdEtatdemandeNavigation.EtatDemande)

               .AsQueryable();
            foreach (DemandeLivraison demande in queryable)
            {
                foreach (FactureClient factureClient in demande.FactureClient)
                {
                    factureClient.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.PayementFile);
                    factureClient.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.FactureFile);
                }

            }


          


            if (filter != 0 && filter1 != 0)
            {
                queryable = _context.DemandeLivraison
               .Include(t => t.DemandeDevis)
               .Include(t => t.Offre)
               .Include(t => t.FactureClient)
               .Include(t => t.IdclientNavigation)
               .ThenInclude(t => t.IduserNavigation)
               .Include(t => t.IdEtatdemandeNavigation)
               .OrderBy(t => t.IdEtatdemandeNavigation.EtatDemande)
               .Where(hh => (hh.IdEtatdemande == filter) || (hh.IdEtatdemande == filter1)).AsQueryable();
                foreach (DemandeLivraison demande in queryable)
                {
                    foreach (FactureClient factureClient in demande.FactureClient)
                    {
                        factureClient.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.FactureFile);

                        factureClient.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.PayementFile);
                    }

                }
            }
            if (filter != 0 && filter1 == 0)
            {
                queryable = _context.DemandeLivraison
               .Include(t => t.DemandeDevis)
               .Include(t => t.Offre)
               .Include(t => t.FactureClient)
               .Include(t => t.IdclientNavigation)
               .ThenInclude(t => t.IduserNavigation)
               .Include(t => t.IdEtatdemandeNavigation)
               .OrderBy(t => t.IdEtatdemandeNavigation.EtatDemande)
               .Where(hh => hh.IdEtatdemande == filter)
               .AsQueryable();
                foreach (DemandeLivraison demande in queryable)
                {
                    foreach (FactureClient factureClient in demande.FactureClient)
                    {
                        factureClient.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.FactureFile);

                        factureClient.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.PayementFile);
                    }

                }
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "oui")
                {
                    queryable = queryable.Where(s => s.FactureClient.Count != 0)
              .AsQueryable();


                }
                if (sortOrder == "non")
                {
                    queryable = queryable.Where(s => s.FactureClient.Count == 0)
               .AsQueryable();
                }
            }
            if (!string.IsNullOrEmpty(date))
            {
                queryable = queryable.Where(s => (s.Date).ToString().Contains(date));
            }

            if (!string.IsNullOrEmpty(name))
            {

                queryable = queryable.Where(s => s.IdclientNavigation.IduserNavigation.Nom.Contains(name) || s.IdclientNavigation.IduserNavigation.Prenom.Contains(name))
                .AsQueryable();


            }
            if (!string.IsNullOrEmpty(num))
            {

                queryable = queryable.Where(s => s.IdDemande.ToString().Contains(num))
                .AsQueryable();


            }
            foreach (DemandeLivraison demande in queryable)
            {
                demande.IdclientNavigation.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, demande.IdclientNavigation.IduserNavigation.Image);
            }

            // queryable = queryable.OrderBy(s => s.FactureClient.Count);
            //var queryable = _context.DemandeLivraison.AsQueryable();
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<DemandeLivraison> demandeLivraisons = await queryable.Paginate(pagination).ToListAsync();

            return demandeLivraisons;






            //**********************************************

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DemandeLivraison>>> GetDemandeLivraison(int id)
        {
            var demandeLivraison = await _context.DemandeLivraison.Where(el => el.IdDemande == id)
                .Include(el => el.IdclientNavigation).ThenInclude(el => el.IduserNavigation)
                .Include(el => el.Offre).ThenInclude(el => el.IdEtatNavigation)
                .Include(el => el.Offre).ThenInclude(el => el.FileOffre)
                .Include(el => el.IdEtatdemandeNavigation)
                .Include(el => el.FileDemandeLivraison)
                .Include(el => el.Offre).ThenInclude(el => el.IdTransporteurNavigation).ThenInclude(el => el.IdUserNavigation).ToListAsync();
            foreach (DemandeLivraison demande in demandeLivraison)
            {
                demande.IdclientNavigation.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, demande.IdclientNavigation.IduserNavigation.Image);
            }
            foreach (FileDemandeLivraison file in demandeLivraison[0].FileDemandeLivraison)
            {
                file.SrcFile = String.Format("{0}://{1}{2}/File/Client/DemandeLivraison/{3}", Request.Scheme, Request.Host, Request.PathBase, file.NomFile);
            }
            foreach (Offre offre in demandeLivraison[0].Offre)
            {
                foreach (FileOffre file in offre.FileOffre)
                {
                    file.SrcOffreFile = String.Format("{0}://{1}{2}/File/TransporteurFiles/OffreFiles/{3}", Request.Scheme, Request.Host, Request.PathBase, file.NomFile);
                }
                //file.SrcFile = String.Format("{0}://{1}{2}/File/Client/DemandeLivraison/{3}", Request.Scheme, Request.Host, Request.PathBase, file.NomFile);
            }
            if (demandeLivraison == null)
            {
                return NotFound();
            }

            return demandeLivraison;
        }



        [HttpGet("demandeall")]
        public async Task<ActionResult<IEnumerable<DemandeLivraison>>> GetDemandeLivraisons()
        {
            return await _context.DemandeLivraison.ToListAsync();
        }



        // GET: api/DemandeLivraisons/5
        [HttpGet("client/{id}")]
        public async Task<ActionResult<IEnumerable<DemandeLivraison>>> GetClientDemandeLivraison([FromQuery] Paginations pagination, [FromQuery] string depart, [FromQuery] string num, [FromQuery] string arrive ,int id)
        {
            var demandeLivraison =  _context.DemandeLivraison.Where(t=>t.IdclientNavigation.Iduser==id).Include(t=>t.IdEtatdemandeNavigation).Include(t=>t.Offre).Include(t=>t.FileDemandeLivraison).AsQueryable();
            
            if (!string.IsNullOrEmpty(depart))
            {
                demandeLivraison = demandeLivraison.Where(t => t.Adressdepart.Contains(depart));
            }
            if (!string.IsNullOrEmpty(arrive))
            {
                demandeLivraison = demandeLivraison.Where(t => t.Adressarrive.Contains(arrive));
            }
            if (!string.IsNullOrEmpty(num))
            {
                demandeLivraison = demandeLivraison.Where(t => t.IdDemande.ToString().Contains(num));
            }
            foreach (DemandeLivraison d in demandeLivraison)
            {
               foreach (FileDemandeLivraison f in d.FileDemandeLivraison)
                {
                    f.SrcFile = String.Format("{0}://{1}{2}/File/Client/DemandeLivraison/{3}", Request.Scheme, Request.Host, Request.PathBase, f.NomFile);
                }
            }
            await HttpContext.InsertPaginationParameterInResponse(demandeLivraison, pagination.QuantityPage);
            List<DemandeLivraison> demandeLivraisons = await demandeLivraison.Paginate(pagination).OrderByDescending(t=>t.IdDemande).ToListAsync();
            if (demandeLivraison == null)
            {
                return NotFound();
            }

            return demandeLivraisons;
        }

        // PUT: api/DemandeLivraisons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDemandeLivraison(int id, DemandeLivraison demandeLivraison)
        {
            if (id != demandeLivraison.IdDemande)
            {
                return BadRequest();
            }

            _context.Entry(demandeLivraison).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemandeLivraisonExists(id))
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

        // POST: api/DemandeLivraisons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DemandeLivraison>> PostDemandeLivraison(DemandeLivraison demandeLivraison)
        {
            _context.DemandeLivraison.Add(demandeLivraison);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDemandeLivraison", new { id = demandeLivraison.IdDemande }, demandeLivraison);
        }

        // DELETE: api/DemandeLivraisons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DemandeLivraison>> DeleteDemandeLivraison(int id)
        {
            var demandeLivraison = await _context.DemandeLivraison.FindAsync(id);
            if (demandeLivraison == null)
            {
                return NotFound();
            }

            _context.DemandeLivraison.Remove(demandeLivraison);
            await _context.SaveChangesAsync();

            return demandeLivraison;
        }

        private bool DemandeLivraisonExists(int id)
        {
            return _context.DemandeLivraison.Any(e => e.IdDemande == id);
        }
    }
}
