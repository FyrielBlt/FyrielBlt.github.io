using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class EtatDemandeDevis
    {
        public EtatDemandeDevis()
        {
            DemandeDevis = new HashSet<DemandeDevis>();
        }

        public int IdEtat { get; set; }
        public string Etat { get; set; }

        public virtual ICollection<DemandeDevis> DemandeDevis { get; set; }
    }
}
