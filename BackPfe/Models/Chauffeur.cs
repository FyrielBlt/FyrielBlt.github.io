using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Chauffeur
    {
        public Chauffeur()
        {
            Camion = new HashSet<Camion>();
        }

        public int Idchauffeur { get; set; }
        public string Cinchauffeur { get; set; }
        public int Idsociete { get; set; }
        public int Iduser { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }

        public virtual Users IduserNavigation { get; set; }
        public virtual ICollection<Camion> Camion { get; set; }
    }
}
