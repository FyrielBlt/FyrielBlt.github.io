using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Users
    {
        public Users()
        {
            Chauffeur = new HashSet<Chauffeur>();
            Client = new HashSet<Client>();
            Intermediaire = new HashSet<Intermediaire>();
            Transporteur = new HashSet<Transporteur>();
        }

        public int IdUser { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Motdepasse { get; set; }
        public string Image { get; set; }
        public int? Societe { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }

        public virtual Societe SocieteNavigation { get; set; }
        public virtual ICollection<Chauffeur> Chauffeur { get; set; }
        public virtual ICollection<Client> Client { get; set; }
        public virtual ICollection<Intermediaire> Intermediaire { get; set; }
        public virtual ICollection<Transporteur> Transporteur { get; set; }
    }
}
