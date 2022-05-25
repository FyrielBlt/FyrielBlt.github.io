using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Intermediaire
    {
        public Intermediaire()
        {
            DemandeDevis = new HashSet<DemandeDevis>();
            Ville = new HashSet<Ville>();
        }

        public int IdIntermediaire { get; set; }
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
        public virtual ICollection<DemandeDevis> DemandeDevis { get; set; }
        public virtual ICollection<Ville> Ville { get; set; }
    }
}
