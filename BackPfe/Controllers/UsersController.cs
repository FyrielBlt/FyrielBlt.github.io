using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackPfe.Models;
using BackPfe.Paginate;
using Microsoft.AspNetCore.Hosting;
using BackPfe.Upload;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BasePfeContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public UsersController(BasePfeContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hostEnvironment = hosting;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {

            return await _context.Users.ToListAsync();

        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }
            users.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host,
                Request.PathBase, users.Image);
            return users;
        }
        // GET: api/Users/5
        [HttpGet("client/{id}")]
        public async Task<ActionResult<IEnumerable<Users>>> GetClientUsers(int id)
        {
            var users = await _context.Users.Where(t => t.Societe == id).ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<Users>> PutUsers(int id, [FromForm] Users users)
        {
            List<Users> test = _context.Users.Where(t => t.Email == users.Email)
                .Where(t => t.IdUser != users.IdUser)
                .ToList();
            if (test.Count == 0)
            {
                if (id != users.IdUser)
                {
                    return BadRequest();
                }
                var user = await _context.Users.FindAsync(id);
                if (users.ImageFile == null)
                {

                    user.Nom = users.Nom;
                    user.Prenom = users.Prenom;
                    user.Email = users.Email;
                    user.Societe = users.Societe;
                    user.Motdepasse = users.Motdepasse;
                    _context.Entry(user).State = EntityState.Modified;
                    user.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, user.Image);

                }
                else
                {
                    UploadFile.DeleteImage(user.Image, _hostEnvironment, "File/Image");
                    user.Image = UploadFile.UploadImage(users.ImageFile, _hostEnvironment, "File/Image");
                    user.Nom = users.Nom;
                    user.Prenom = users.Prenom;
                    user.Email = users.Email;
                    user.Societe = users.Societe;
                    user.Motdepasse = users.Motdepasse;
                    _context.Entry(user).State = EntityState.Modified;
                    user.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, user.Image);

                }



                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


                if (user == null)
                {
                    return NotFound();
                }

                return user;

            }
            else
            {
                return NotFound("Email doit etre unique");
            }







        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers([FromForm] Users users)
        {
            // upload image dans File/Image + return name de L'Image 
            List<Users> test = _context.Users.Where(t => t.Email == users.Email).ToList();
            if (test.Count == 0)
            {
                users.Image = UploadFile.UploadImage(users.ImageFile, _hostEnvironment, "File/Image");
                _context.Users.Add(users);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUsers", new
                {
                    idUser = users.IdUser,
                    nom = users.Nom,
                    prenom = users.Prenom,
                    email = users.Email,
                    motdepasse = users.Motdepasse,
                    societe = users.Societe,
                    image = users.Image,
                    srcOffreFile = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host,
                    Request.PathBase, users.Image),

                });
            }
            else
            {
                return NotFound("Email doit etre unique");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            UploadFile.DeleteImage(users.Image, _hostEnvironment, "File/Image");
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }
    }
}