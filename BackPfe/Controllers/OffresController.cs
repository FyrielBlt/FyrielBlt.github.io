﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using BackPfe.Paginate;
using System.Net.Mail;

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

        // GET: api/Offres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offre>>> GetOffre()
        {
            return await _context.Offre.ToListAsync();
        }

        // GET: api/Offres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Offre>>> GetOffre(int id)
        {
           
                var offre = await _context.Offre.Where(t => t.IdOffre == id).
                Include(t => t.IdDemandeNavigation)
                .Include(t => t.IdEtatNavigation).Include(t => t.IdTransporteurNavigation).ThenInclude(t => t.IdUserNavigation).ToListAsync();

                if (offre == null)
                {
                    return NotFound();
                }

                return offre;
            
        }
        [HttpGet("{id}/idcamion")]
        public async Task<ActionResult<IEnumerable<Offre>>> GetOffrebyidtrajet(int id)
        {

            var offre = await _context.Offre.Where(t => t.IdCamion == id).
            Include(t => t.IdDemandeNavigation)
            .Include(t => t.IdEtatNavigation).Include(t => t.IdTransporteurNavigation).ThenInclude(t => t.IdUserNavigation).ToListAsync();

            if (offre == null)
            {
                return NotFound();
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
                .Include(t=>t.IdDemandeNavigation)
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
                 s.Prix.ToString().Contains(search)

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

                if (etat == "Refuse")
                {
                    offre = offre.Where(s =>
                    s.IdEtatNavigation.Etat.Contains("Réfusé"));
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

            if (offre == null)
            {
                return NotFound();
            }

            return offres;

        }
        [HttpGet("client/{id}")]
        public async Task<ActionResult<IEnumerable<Offre>>> GetClientOffre([FromQuery] Paginations pagination, [FromQuery] string depart, [FromQuery] string arrive, int id)
        {
            var offre =  _context.Offre.Where(t=>t.IdDemandeNavigation.IdclientNavigation.Iduser==id).Include(t=>t.IdDemandeNavigation).ThenInclude(t=>t.IdclientNavigation).ThenInclude(t=>t.IduserNavigation).Include(t => t.IdEtatNavigation).Include(t => t.IdTransporteurNavigation).ThenInclude(t=>t.IdUserNavigation).AsQueryable();
            if (!string.IsNullOrEmpty(depart) && string.IsNullOrEmpty(arrive))
            {
                offre =offre.Where(t=>t.IdDemandeNavigation.Adressdepart.Contains(depart));    
            }else if(!string.IsNullOrEmpty(arrive) && string.IsNullOrEmpty(depart))
            {
                offre = offre.Where(t => t.IdDemandeNavigation.Adressarrive.Contains(arrive));
            }else if(!string.IsNullOrEmpty(depart)&& !string.IsNullOrEmpty(arrive))
            {
                offre = offre.Where(t => t.IdDemandeNavigation.Adressarrive.Contains(arrive) && t.IdDemandeNavigation.Adressdepart.Contains(depart));
            }
                await HttpContext.InsertPaginationParameterInResponse(offre, pagination.QuantityPage);
            List<Offre> offres = await offre.Paginate(pagination).ToListAsync();
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
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("beltaiefferiel98@gmail.com");
                    mail.To.Add("ferielcontact.email@gmail.com");
                    mail.Subject = "Une modification sur l'offre";
                    mail.Body = "Bonjour , une offre a été envoyer, Notez bien si vous ne répondrez pas dans le bien délai " +
                        "l'offre sera réjuter ";
                    mail.IsBodyHtml = true;
                    using (SmtpClient stmp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        stmp.Credentials = new System.Net.NetworkCredential("beltaiefferiel98@gmail.com", "ferielsansa01052018*0");
                        stmp.EnableSsl = true;
                        stmp.Send(mail);
                    }

                }

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
                mail.From = new MailAddress("beltaiefferiel98@gmail.com");
                mail.To.Add("malek@gmail.com");
                mail.Subject = "Une réponse a votre demande a été envoyer";
                mail.Body = "Bonjour , une offre est disponible";
                mail.IsBodyHtml = true;
                using (SmtpClient stmp = new SmtpClient("smtp.gmail.com", 587))
                {
                    stmp.Credentials = new System.Net.NetworkCredential("beltaiefferiel98@gmail.com", "ferielsansa01052018*0");
                    stmp.EnableSsl = true;
                    stmp.Send(mail);
                }

            }
            return CreatedAtAction("GetOffre", new { id = offre.IdOffre }, offre);
        }

        // DELETE: api/Offres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Offre>> DeleteOffre(int id)
        {
            var offre = await _context.Offre.FindAsync(id);
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
