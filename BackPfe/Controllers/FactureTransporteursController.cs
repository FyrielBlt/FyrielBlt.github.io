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
using BackPfe.Paginate;
using System.Net.Mail;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactureTransporteursController : ControllerBase
    {
        private readonly BasePfeContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FactureTransporteursController(BasePfeContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hostEnvironment = hosting;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactureTransporteur>>> GetFactureTransporteur([FromQuery] Paginations pagination, [FromQuery] string num, [FromQuery] string sortOrder)
        {
            var queryable = _context.FactureTransporteur
                .Include(el => el.IdOffreNavigation)
                .ThenInclude(el => el.IdDemandeNavigation)
                .Include(el => el.IdOffreNavigation)
                .ThenInclude(el => el.IdTransporteurNavigation)
                .ThenInclude(el => el.IdUserNavigation).AsQueryable();
            foreach (FactureTransporteur facture in queryable)
            {
                facture.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, facture.PayementFile);
                facture.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, facture.FactureFile);
                facture.IdOffreNavigation.IdTransporteurNavigation.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, facture.IdOffreNavigation.IdTransporteurNavigation.IdUserNavigation.Image);
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "oui")
                {
                    queryable = queryable.Where(s => s.PayementFile != null);
                }
                if (sortOrder == "non")
                {
                    queryable = queryable.Where(s => s.PayementFile == null);
                }
                if (sortOrder != "non" && sortOrder != "oui" && sortOrder != "0")
                {
                    queryable = queryable.Where(s => s.IdOffreNavigation.IdTransporteurNavigation.IdUserNavigation.Nom.Contains(sortOrder) ||
               s.IdOffreNavigation.IdTransporteurNavigation.IdUserNavigation.Prenom.Contains(sortOrder));

                }
            }
            if (!string.IsNullOrEmpty(num))
            {

                queryable = queryable.Where(s => s.IdFactTransporteur.ToString().Contains(num));


            }
            queryable = queryable.OrderBy(s => s.PayementFile);

            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.QuantityPage);
            //element par page
            List<FactureTransporteur> factureTransporteurs = await queryable.Paginate(pagination).ToListAsync();

            return factureTransporteurs;
            // return queryable;
        }

        // GET: api/FactureTransporteurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FactureTransporteur>> GetFactureTransporteur(int id)
        {
            var factureTransporteur = await _context.FactureTransporteur.FindAsync(id);

            if (factureTransporteur == null)
            {
                return NotFound();
            }
            factureTransporteur.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, factureTransporteur.FactureFile);
            factureTransporteur.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, factureTransporteur.PayementFile);

            return factureTransporteur;
        }

        // PUT: api/FactureTransporteurs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [RequestSizeLimit(524288000)]
        public async Task<ActionResult<FactureTransporteur>> PutFactureTransporteur(int id, [FromForm]FactureTransporteur factureTransporteur)
        {
            if (id != factureTransporteur.IdFactTransporteur)
            {
                return BadRequest();
            }
             factureTransporteur.PayementFile = UploadFile.UploadImage(factureTransporteur.ImageFile, _hostEnvironment, "File/IntermediaireFile/factureTransporteur");
            _context.Entry(factureTransporteur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureTransporteurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            factureTransporteur.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, factureTransporteur.FactureFile);
            factureTransporteur.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, factureTransporteur.PayementFile);


            // return facture;
            return factureTransporteur;
        }

        // POST: api/FactureTransporteurs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FactureTransporteur>> PostFactureTransporteur( [FromForm] FactureTransporteur factureTransporteur)
        {

            factureTransporteur.FactureFile = UploadFile.UploadImage(factureTransporteur.ImageFile, _hostEnvironment,
                "File/IntermediaireFile/factureTransporteur/");
                 _context.FactureTransporteur.Add(factureTransporteur);
                 await _context.SaveChangesAsync();
          /*  if (factureTransporteur.PayementFile ==null)
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("beltaiefferiel98@gmail.com");
                    mail.To.Add("malekbouzayani9@gmail.com");
                    mail.Subject = "Facture Transport ajouté";
                    mail.IsBodyHtml = true;
                    string htmlBody = "<img style ='border-radius: 50px' src = 'https://scontent.ftun15-1.fna.fbcdn.net/v/t1.18169-9/21371290_844588729051616_3220980191669635459_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=09cbfe&_nc_ohc=z_UPPke7CwsAX_GUsgk&_nc_ht=scontent.ftun15-1.fna&oh=00_AT_aJCN0JnSMKh_DPnDU4fXe2nIF4R3AzTAEJvs7a-6Cyg&oe=62BDF38A' alt = 'Image Profil' /> " +
                        "<h2>Nous tenons à vous informer que votre commande a été livré avec succés.<br>" +
                  "Nous avons envoyé la facture.</h2> <br><br> Cordialement,";
                    mail.Body = htmlBody;
                    using (SmtpClient stmp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        stmp.Credentials = new System.Net.NetworkCredential("beltaiefferiel98@gmail.com", "ferielsansa01052018*0");
                        stmp.EnableSsl = true;
                        stmp.Send(mail);
                    }

                
            }*/
            return CreatedAtAction("GetFactureTransporteur", new
            {
                IdFactTransporteur =factureTransporteur.IdFactTransporteur,
                IdOffre =factureTransporteur.IdOffre,
                FactureFile=factureTransporteur.FactureFile,
                PayementFile=factureTransporteur.PayementFile,
                Notification=factureTransporteur.Notification,
                SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host,
                Request.PathBase, factureTransporteur.FactureFile),

            });

           
        }

        // DELETE: api/FactureTransporteurs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FactureTransporteur>> DeleteFactureTransporteur(int id)
        {
            var factureTransporteur = await _context.FactureTransporteur.FindAsync(id);
            if (factureTransporteur == null)
            {
                return NotFound();
            }

            _context.FactureTransporteur.Remove(factureTransporteur);
            await _context.SaveChangesAsync();

            return factureTransporteur;
        }

        private bool FactureTransporteurExists(int id)
        {
            return _context.FactureTransporteur.Any(e => e.IdFactTransporteur == id);
        }
    }
}
