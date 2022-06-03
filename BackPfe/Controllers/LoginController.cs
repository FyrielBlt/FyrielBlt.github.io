using BackPfe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        private readonly BasePfeContext _context;
        private IConfiguration _config;
        public LoginController(BasePfeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }



        //authentification *******************************

        [HttpPost]
        public ActionResult<Users> CreateToken(Users login)
        {
            if (login == null) return Unauthorized();
            string tokenString = string.Empty;

            //Users validUser = _context.Users.Where(t => (t.Email == login.Email) && (t.Motdepasse == login.Motdepasse)).First(); ;
            Users validUser = Authenticate(login);
            string type = Type(validUser);
            if (validUser != null)
            {
                tokenString = BuildJWTToken();
            }
            else
            {
                return Unauthorized();
            }
            return Ok(new { Token = tokenString, validUser, Type = type });
        }

        private string BuildJWTToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var issuer = _config["JwtToken:Issuer"];
            var audience = _config["JwtToken:Audience"];
            var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtToken:TokenExpiry"]));

            var token = new JwtSecurityToken(issuer,
              audience,
              expires: jwtValidity,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private Users Authenticate(Users login)
        {
            Users validUser = null;
            List<Users> users = _context.Users.ToList();
            foreach (Users user in users)
            {
                if ((user.Email == login.Email) && (user.Motdepasse == login.Motdepasse) &&(user.Active==1))
                {
                    validUser = user;
                    validUser.ImageSrc = String.Format("{0}://{1}{2}/File/Image/{3}", Request.Scheme, Request.Host, Request.PathBase, validUser.Image);
                }
            }
            if (validUser != null) { 
            var client =  _context.Client.Where(el=>el.Iduser== validUser.IdUser);
            }
            return validUser;

        }
        private string Type(Users users)
        {
            if (users != null)
            {
                _context.Entry(users)
                   .Collection(sta => sta.Client)
                   .Load();
                _context.Entry(users)
                   .Collection(sta => sta.Transporteur)
                   .Load();
                _context.Entry(users)
                   .Collection(sta => sta.Intermediaire)
                   .Load();
               /* var client = _context.Users.Include(el=>el.Client).Where(el => el.IdUser == users.IdUser);
                var transporteur = _context.Transporteur.Where(el => el.IdUser == users.IdUser);
                var intermediaire = _context.Intermediaire.Where(el => el.IdUser == users.IdUser);*/
                if(users.Client.Count!= 0 && users.Transporteur.Count == 0 && users.Intermediaire.Count == 0 && users.Active==1)
                {
                    return "client";
                }
                else if (users.Client.Count == 0 && users.Transporteur.Count != 0 && users.Intermediaire.Count == 0 && users.Active == 1)
                {
                    return "transporteur";
                }
                else if (users.Client.Count == 0 && users.Transporteur.Count == 0 && users.Intermediaire.Count != 0)
                {
                    return "intermediaire";

                }

                else
                {
                    return "";
                }
                
            }
            else
            {
                return "";
            }

        }







        //authentification *******************************
    }
}
