using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Camion
    {
        public Camion()
        {
            Trajet = new HashSet<Trajet>();
        }

        public int Idcamion { get; set; }
        public string Codevehicule { get; set; }
        public int Idtransporteur { get; set; }
        public int? Idchauffeur { get; set; }
        public int? Idtype { get; set; }

        public virtual Chauffeur IdchauffeurNavigation { get; set; }
        public virtual Transporteur IdtransporteurNavigation { get; set; }
        public virtual TypeCamion IdtypeNavigation { get; set; }
        public virtual ICollection<Trajet> Trajet { get; set; }
    }
}
