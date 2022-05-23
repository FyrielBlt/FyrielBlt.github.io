using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class EtatDemandeLivraison
    {
        public EtatDemandeLivraison()
        {
            DemandeLivraison = new HashSet<DemandeLivraison>();
        }

        public int IdEtatDemande { get; set; }
        public string EtatDemande { get; set; }

        public virtual ICollection<DemandeLivraison> DemandeLivraison { get; set; }
    }
}
