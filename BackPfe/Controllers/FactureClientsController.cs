﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using Microsoft.AspNetCore.Hosting;
using BackPfe.Upload;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactureClientsController : ControllerBase
    {
        private readonly BasePfeContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public FactureClientsController(BasePfeContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hostEnvironment = hosting;
        }

        // GET: api/FactureClients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactureClient>>> GetFactureClient()
        {
            return await _context.FactureClient.ToListAsync();
        }

        // GET: api/FactureClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FactureClient>> GetFactureClient(int id)
        {
            var factureClient = await _context.FactureClient.FindAsync(id);

            if (factureClient == null)
            {
                return NotFound();
            }
            factureClient.SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.FactureFile);
            factureClient.SrcPayementFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.PayementFile);
            return factureClient;
        }

        // PUT: api/FactureClients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactureClient(int id, FactureClient factureClient)
        {
            if (id != factureClient.IdFactClient)
            {
                return BadRequest();
            }

            _context.Entry(factureClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureClientExists(id))
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

        // POST: api/FactureClients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost]
        [RequestSizeLimit(524288000)]
        public async Task<ActionResult<FactureClient>> PostFactureClient([FromForm] FactureClient factureClient)
        {
           /* _context.FactureClient.Add(factureClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFactureClient", new { id = factureClient.IdFactClient }, factureClient);
           */



            factureClient.FactureFile = UploadFile.UploadImage(factureClient.ImageFile, _hostEnvironment, "File/IntermediaireFile/factureClient");
            _context.FactureClient.Add(factureClient);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFactureClient", new
            {
                IdFactClient = factureClient.IdFactClient,
                EtatFacture = factureClient.EtatFacture,
                IdDemandeLivraison = factureClient.IdDemandeLivraison,
                FactureFile = factureClient.FactureFile,
                PayementFile = factureClient.PayementFile,
                SrcFactureFile = String.Format("{0}://{1}{2}/File/IntermediaireFile/factureClient/{3}", Request.Scheme, Request.Host, Request.PathBase, factureClient.FactureFile),
                
            });
        }

        // DELETE: api/FactureClients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FactureClient>> DeleteFactureClient(int id)
        {
            var factureClient = await _context.FactureClient.FindAsync(id);
            if (factureClient == null)
            {
                return NotFound();
            }

            _context.FactureClient.Remove(factureClient);
            await _context.SaveChangesAsync();

            return factureClient;
        }

        private bool FactureClientExists(int id)
        {
            return _context.FactureClient.Any(e => e.IdFactClient == id);
        }
    }
}