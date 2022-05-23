using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class EtatOffre
    {
        public EtatOffre()
        {
            Offre = new HashSet<Offre>();
        }

        public int IdEtat { get; set; }
        public string Etat { get; set; }

        public virtual ICollection<Offre> Offre { get; set; }
    }
}
