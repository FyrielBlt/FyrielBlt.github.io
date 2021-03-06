using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using BackPfe.Paginate;
using System.Net.Mail;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffresController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public OffresController(BasePfeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offre>>> GetOffre()
        {
            return await _context.Offre.Include(t => t.FileOffre).
                                Include(t => t.IdDemandeNavigation).ThenInclude(t => t.DemandeDevis).

                ToListAsync();
        }

        // GET: api/Offres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Offre>>> GetOffre(int id)
        {
           
                var offre = await _context.Offre.Where(t => t.IdOffre == id).
                Include(t => t.IdDemandeNavigation)
                .Include(t=>t.FileOffre)
                .Include(t => t.IdEtatNavigation).Include(t => t.IdTransporteurNavigation).ThenInclude(t => t.IdUserNavigation).ToListAsync();

                if (offre == null)
                {
                    return NotFound();
                }

                return offre;
            
        }
        // GET: api/Offres/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Offre>> GetOffreDetails(int id)
        {

            var offre = await _context.Offre.Where(t => t.IdDemandeNavigation.IdDemande == id)
                .Include(t => t.IdDemandeNavigation).ThenInclude(t=>t.IdEtatdemandeNavigation)
                .Include(t => t.IdDemandeNavigation).ThenInclude(t => t.FileDemandeLivraison)
                .Include(t => t.IdEtatNavigation)
                .Include(t => t.IdTransporteurNavigation).ThenInclude(t => t.IdUserNavigation)
                .Include(t=>t.FileOffre)
                .FirstAsync();


            if (offre == null)
            {
                return NotFound();
            }

            offre.IdTransporteurNavigation.IdUserNavigation.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host,
                    Request.PathBase, offre.IdTransporteurNavigation.IdUserNavigation.Image);
            foreach (FileOffre f in offre.FileOffre)
            {
                f.SrcOffreFile = String.Format("{0}://{1}{2}/File/TransporteurFiles/OffreFiles/{3}", Request.Scheme, Request.Host, Request.PathBase, f.NomFile);
            }
            foreach (FileDemandeLivraison f in offre.IdDemandeNavigation.FileDemandeLivraison)
            {
                f.SrcFile = String.Format("{0}://{1}{2}/File/Client/DemandeLivraison/{3}", Request.Scheme, Request.Host, Request.PathBase, f.NomFile);
            }
            return offre;
        }

        [HttpGet("{id}/offrestransporteur")]
        public async Task<ActionResult<IEnumerable<Offre>>> GetOffrebyidtransporteur(
             [FromQuery] Paginations pagination,
             [FromQuery] string depart,
             [FromQuery] string arrive,
             [FromQuery] string date,
             [FromQuery] string etat,
             [FromQuery] string search,
            int id)
        {

            var offre =  _context.Offre.Where(t => t.IdTransporteur == id)
                .Include(t => t.IdEtatNavigation)
                .Include(t => t.FileOffre)
                .Include(t=>t.FactureTransporteur)

                .Include(t=>t.IdDemandeNavigation).ThenInclude(t=>t.DemandeDevis)
                .OrderByDescending
               (s => s.IdOffre)
                .AsQueryable();
            if (!string.IsNullOrEmpty(arrive))
            {
                offre = offre.Where(s =>
                 s.IdDemandeNavigation.Adressarrive .Contains(arrive)

                );
            }
            if (!string.IsNullOrEmpty(search))
            {
                offre = offre.Where(s =>
                 s.IdOffre.ToString()==(search) ||
                  s.IdDemandeNavigation.IdDemande.ToString().Contains(search)
                );
            }





            if (!string.IsNullOrEmpty(depart))
            {
                offre = offre.Where(s =>
                 s.IdDemandeNavigation.Adressdepart.Contains(depart)

                );
            }
            if (!string.IsNullOrEmpty(etat))
            {
                if (etat == "Accepte")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Accepté"));
                }
                if (etat == "Livre")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Livré"));
                }

                if (etat == "Refuse")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Refusé"));
                }
                if (etat == "Livre")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Livré"));
                }
                if (etat == "Non traite")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Non traité"));
                }
                if (etat == "En cours de traitement")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("En cours de traitement"));
                }
                if (etat == "indisponible")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Indisponible"));
                }

            }
            if (!string.IsNullOrEmpty(date))
            {
                offre = offre.Where(s =>
                 s.Date.ToString().Contains(date)

                );
            }

            await HttpContext.InsertPaginationParameterInResponse(offre, pagination.QuantityPage);
            //element par page
            List<Offre> offres= await offre.Paginate(pagination).ToListAsync();
            foreach (Offre o in offre)
            {
                foreach (FactureTransporteur ft in o.FactureTransporteur)
                {
                    ft.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, ft.FactureFile);
                    ft.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureTransporteur/{3}", Request.Scheme, Request.Host, Request.PathBase, ft.PayementFile);
                }
                foreach (FileOffre f in o.FileOffre)
                {
                    f.SrcOffreFile = String.Format("{0}://{1}{2}/File/TransporteurFiles/OffreFiles/{3}", Request.Scheme, Request.Host, Request.PathBase, f.NomFile);
                }

            }
            if (offre == null)
            {
                return NotFound();
            }

            return offres;

        }
        [HttpGet("client/{id}")]
        public async Task<ActionResult<IEnumerable<Offre>>> GetClientOffre([FromQuery] Paginations pagination, [FromQuery] string depart, [FromQuery] string num, [FromQuery] string arrive, int id)
        {
            var offre =  _context.Offre.Where(t=>t.IdDemandeNavigation.IdclientNavigation.Iduser==id)
                .Where(t => t.PrixFinale!=null)
                .OrderByDescending(t=>t.PrixFinale)
                .Include(t=>t.IdDemandeNavigation)
                .ThenInclude(t=>t.IdclientNavigation)
                .ThenInclude(t=>t.IduserNavigation)
                .Include(t => t.IdEtatNavigation)
                .Include(t => t.IdTransporteurNavigation)
                .ThenInclude(t=>t.IdUserNavigation).Include(t=>t.FileOffre)
                .AsQueryable();
            if (!string.IsNullOrEmpty(depart))
            {
                offre =offre.Where(t=>t.IdDemandeNavigation.Adressdepart.Contains(depart));    
            }
            if(!string.IsNullOrEmpty(arrive))
            {
                offre = offre.Where(t => t.IdDemandeNavigation.Adressarrive.Contains(arrive));
            }
            if (!string.IsNullOrEmpty(num))
            {
                offre = offre.Where(t => t.IdDemandeNavigation.IdDemande.ToString().Contains(num));
            }

            await HttpContext.InsertPaginationParameterInResponse(offre, pagination.QuantityPage);
            List<Offre> offres = await offre.Paginate(pagination).OrderByDescending(t => t.IdOffre).ToListAsync();
            foreach (Offre o in offres)
            {
                foreach (FileOffre f in o.FileOffre)
                {
                    f.SrcOffreFile = String.Format("{0}://{1}{2}/File/TransporteurFiles/OffreFiles/{3}", Request.Scheme, Request.Host, Request.PathBase, f.NomFile);
                }
            }
            if (offre == null)
            {
                return NotFound();
            }

            return offres;
        }
        [HttpGet("client/search/{search}")]
        public async Task<ActionResult<IEnumerable<Offre>>> SearchOffre(string search)
        {
            
            var offre = await _context.Offre.Where(s => s.Description.Contains(search)
                               ).Include(t=>t.IdDemandeNavigation).ToListAsync();

            if (offre == null)
            {
                return NotFound();
            }

            return offre;
        }

        // PUT: api/Offres/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffre(int id, Offre offre)
        {

            if (id != offre.IdOffre)
            {
                return BadRequest();
            }

            _context.Entry(offre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OffreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            {
                var accountSid = "AC31e51268bbd5569250e5f5681a4779e2";
                var authToken = "110ee2c78dda43a6c1d160d75af15354";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "Bonjour , une offre a été envoyer, Notez bien si vous ne répondrez pas dans le bien délai " +
                         "l'offre sera rejeter",
                    from: new Twilio.Types.PhoneNumber("+19894873673"),
                    to: new Twilio.Types.PhoneNumber("+21620969466")
                );
                

                /* using (MailMessage mail = new MailMessage())
                 {
                     mail.From = new MailAddress("beltaiefferiel98@gmail.com");
                     mail.To.Add("ferielcontact.email@gmail.com");
                     mail.Subject = "Une modification sur l'offre";
                     mail.Body = "Bonjour , une offre a été envoyer, Notez bien si vous ne répondrez pas dans le bien délai " +
                         "l'offre sera réjuter ";
                     mail.IsBodyHtml = true;
                     using (SmtpClient stmp = new SmtpClient("smtp.mailtrap.io", 2525))
                     {
                         stmp.Credentials = new System.Net.NetworkCredential("9fd3fb2f9c2ff4", "39a7a0d5ed76fd");
                         stmp.EnableSsl = true;
                         stmp.Send(mail);
                     }

                 }*/

            }

            return NoContent();
        }

        // POST: api/Offres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost()]
        public async Task<ActionResult<Offre>> PostOffre(Offre offre)
        {
            _context.Offre.Add(offre);
            await _context.SaveChangesAsync();
           using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("Karimomrane7@gmail.com");
                mail.To.Add("Easy_Order@gmail.com");
                mail.Subject = "Nouvelle Offre";
                mail.IsBodyHtml = true;
                string htmlBody = "<img style ='border-radius: 50px' src = 'https://scontent.ftun15-1.fna.fbcdn.net/v/t1.18169-9/21371290_844588729051616_3220980191669635459_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=09cbfe&_nc_ohc=z_UPPke7CwsAX_GUsgk&_nc_ht=scontent.ftun15-1.fna&oh=00_AT_aJCN0JnSMKh_DPnDU4fXe2nIF4R3AzTAEJvs7a-6Cyg&oe=62BDF38A' alt = 'Image Profil' /> " +
                    "<h2>Nous tenons à vous informer qu'une nouvelle offre vient de vous être envoyée.<br>" +
              "Assurez - vous de le traiter dès que possible.</h2>  Description : "+offre.Description+ "<br>Prix :  " + offre.Prix + " <br> Cordialement,<br>";
                mail.Body = htmlBody;
                using (SmtpClient stmp = new SmtpClient("smtp.mailtrap.io", 2525))
                {
                    stmp.Credentials = new System.Net.NetworkCredential("9fd3fb2f9c2ff4", "39a7a0d5ed76fd");
                    stmp.EnableSsl = true;
                    stmp.Send(mail);
                }

            }
            var accountSid = "AC921a49972bd69488762dd93ce0474cd2";
            var authToken = "de519647c1a2a2e907e03947b1873c29";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Bonjour , une offre a été envoyer, Notez bien si vous ne répondrez pas dans le bien délai " +
                     "l'offre sera rejeter",
                from: new Twilio.Types.PhoneNumber("+14703090378"),
                to: new Twilio.Types.PhoneNumber("+21621433145")
            );

            return CreatedAtAction("GetOffre", new { id = offre.IdOffre,
            }, offre);
        }

        // DELETE: api/Offres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Offre>> DeleteOffre(int id)
        {
            var offre = await _context.Offre.
                
                FindAsync(id);
            if (offre == null)
            {
                return NotFound();
            }

            _context.Offre.Remove(offre);
            await _context.SaveChangesAsync();

            return offre;
        }

        private bool OffreExists(int id)
        {
            return _context.Offre.Any(e => e.IdOffre == id);
        }
    }
}
