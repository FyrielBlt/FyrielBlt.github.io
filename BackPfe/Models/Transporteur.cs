using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations.Schema;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Transporteur
    {
        public Transporteur()
        {
            Camion = new HashSet<Camion>();
            DemandeDevis = new HashSet<DemandeDevis>();
            Offre = new HashSet<Offre>();
        }

        public int IdTransporteur { get; set; }
        public int IdUser { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]

        public string ImageSrc { get; set; }
        public virtual Users IdUserNavigation { get; set; }
        public virtual ICollection<Camion> Camion { get; set; }
        public virtual ICollection<DemandeDevis> DemandeDevis { get; set; }
        public virtual ICollection<Offre> Offre { get; set; }
    }
}