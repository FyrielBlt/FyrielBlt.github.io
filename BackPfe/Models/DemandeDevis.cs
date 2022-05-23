using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class DemandeDevis
    {
        public int IdDemandeDevis { get; set; }
        public DateTime DateEnvoit { get; set; }
        public int IdIntermediaire { get; set; }
        public int IdDemande { get; set; }
        public int IdTransporteur { get; set; }
        public int IdEtat { get; set; }

        public virtual DemandeLivraison IdDemandeNavigation { get; set; }
        public virtual EtatDemandeDevis IdEtatNavigation { get; set; }
        public virtual Intermediaire IdIntermediaireNavigation { get; set; }
        public virtual Transporteur IdTransporteurNavigation { get; set; }
    }
}
