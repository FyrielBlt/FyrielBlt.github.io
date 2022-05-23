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
    public class ChauffeursController : ControllerBase
    {
        private readonly BasePfeContext _context;

        public ChauffeursController(BasePfeContext context)
        {
            _context = context;
        }

        // GET: api/Chauffeurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chauffeur>>> GetChauffeurs()
        {
            return await _context.Chauffeur
                .ToListAsync();
        }
        [HttpGet("{id}/idchauffeur")]
        public async Task<ActionResult<Chauffeur>> GetChauff(int id)
        {
            //  var users = await _context.Users.FindAsync(id);
            Chauffeur chauffeurs = await _context.Chauffeur.Where(t => t.Idchauffeur == id)
                 .Include(t => t.Camion)
                 .FirstAsync();

            if (chauffeurs == null)
            {
                return NotFound();
            }

            return chauffeurs;
        }
        // GET: api/Chauffeurs/5
        [HttpGet("{cin}")]
        public async Task<ActionResult<Chauffeur>> GetChauffeur(string cin)
        {
            var chauffeur = _context.Chauffeur.Where(t => t.Cinchauffeur == cin)
               .First();
            chauffeur.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host,
                Request.PathBase, chauffeur.IduserNavigation.Image)
                ;
            if (chauffeur == null)
            {
                return NotFound();
            }


            return chauffeur;
        }
        [HttpGet("{societe}/chauffeurs")]
        public async Task<ActionResult<IEnumerable<Chauffeur>>> GetChauffeurbysociete(
            [FromQuery] Paginations pagination, [FromQuery] string search, int societe)
        {



            var chauffeur = _context.Chauffeur.Where(t => t.Idsociete == societe)
                .Include(t => t.IduserNavigation).Include(t => t.Camion)
                .ThenInclude(t => t.Trajet).ThenInclude(t => t.IdVille2Navigation)
             .Select(x => new Chauffeur()
             {
                 Idchauffeur = x.Idchauffeur,
                 Iduser = x.Iduser,
                 Cinchauffeur = x.Cinchauffeur,
                 IduserNavigation = x.IduserNavigation,
                 ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, x.IduserNavigation.Image)
             })

                 .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                chauffeur = chauffeur.Where(s => s.Cinchauffeur.Contains(search)
                || s.IduserNavigation.Nom.Contains(search)
                || s.IduserNavigation.Prenom.Contains(search)
                || s.IduserNavigation.Email.Contains(search)
                );
            }
            //ajout nombre de page
            await HttpContext.InsertPaginationParameterInResponse(chauffeur, pagination.QuantityPage);
            //element par page
            List<Chauffeur> chauffeurs = await chauffeur.Paginate(pagination).ToListAsync();

            return chauffeurs;

        }


        // PUT: api/Chauffeurs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChauffeurs(int id, Chauffeur chauffeurs)
        {
            List<Chauffeur> test = _context.Chauffeur.Where(t => t.Cinchauffeur == chauffeurs.Cinchauffeur)
                .Where(t => t.Idchauffeur != chauffeurs.Idchauffeur)
                .ToList();
            if (test.Count == 0)
            {
                if (id != chauffeurs.Idchauffeur)
                {
                    return BadRequest();
                }

                _context.Entry(chauffeurs).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChauffeursExists(id))
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

            else
            {
                return NotFound("Cin doit etre unique");
            }
        }

        // POST: api/Chauffeurs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{email}")]
        public async Task<ActionResult<Chauffeur>> PostChauffeurs(Chauffeur chauffeurs, string email)
        {
            List<Chauffeur> test = _context.Chauffeur.Where(t => t.Cinchauffeur == chauffeurs.Cinchauffeur).ToList();
            if (test.Count == 0)
            {
                _context.Chauffeur.Add(chauffeurs);
                await _context.SaveChangesAsync();
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("beltaiefferiel98@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Votre Compte Chauffeur a été bienc créé";
                    mail.Body = "Pour contacter a votre compte rendez vous sur le site : ";
                    mail.IsBodyHtml = true;
                    using (SmtpClient stmp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        stmp.Credentials = new System.Net.NetworkCredential("beltaiefferiel98@gmail.com", "ferielsansa01052018*0");
                        stmp.EnableSsl = true;
                        stmp.Send(mail);
                    }

                }
                return CreatedAtAction("GetChauffeurs", new { id = chauffeurs.Idchauffeur }, chauffeurs);
            }

            return NotFound("Cin doit etre unique");
        }

        // DELETE: api/Chauffeurs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Chauffeur>> DeleteChauffeurs(int id)
        {
            var chauffeurs = await _context.Chauffeur.FindAsync(id);
            List<Users> users = _context.Users.Where(x => x.IdUser == chauffeurs.Iduser).ToList();
            foreach (Users t in users)
            {
                _context.Users.Remove(t);

            }

            if (chauffeurs == null)
            {
                return NotFound();
            }

            _context.Chauffeur.Remove(chauffeurs);
            await _context.SaveChangesAsync();

            return chauffeurs;
        }

        private bool ChauffeursExists(int id)
        {
            return _context.Chauffeur.Any(e => e.Idchauffeur == id);
        }
    }
}