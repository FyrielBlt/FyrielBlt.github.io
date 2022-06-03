using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Offre
    {
        public Offre()
        {
            FactureTransporteur = new HashSet<FactureTransporteur>();
            FileOffre = new HashSet<FileOffre>();
        }

        public int IdOffre { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int IdEtat { get; set; }
        public int Prix { get; set; }
        public int? PrixFinale { get; set; }
        public int IdTransporteur { get; set; }
        public int IdDemande { get; set; }
        public TimeSpan? Heurearrive { get; set; }
        public int? NotificationIntermediaire { get; set; }
        public int? NotificationClient { get; set; }
        public int? NotificationTransporteur { get; set; }
        public DateTime? Datecreation { get; set; }

        public virtual DemandeLivraison IdDemandeNavigation { get; set; }
        public virtual EtatOffre IdEtatNavigation { get; set; }
        public virtual Transporteur IdTransporteurNavigation { get; set; }
        public virtual ICollection<FactureTransporteur> FactureTransporteur { get; set; }
        public virtual ICollection<FileOffre> FileOffre { get; set; }
    }
}
