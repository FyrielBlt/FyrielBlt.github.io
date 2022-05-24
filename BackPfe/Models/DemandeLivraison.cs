using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class DemandeLivraison
    {
        public DemandeLivraison()
        {
            DemandeDevis = new HashSet<DemandeDevis>();
            FactureClient = new HashSet<FactureClient>();
            FileDemandeLivraison = new HashSet<FileDemandeLivraison>();
            Offre = new HashSet<Offre>();
        }

        public int IdDemande { get; set; }
        public string Description { get; set; }
        public DateTime Datecreation { get; set; }
        public string Adressdepart { get; set; }
        public string Adressarrive { get; set; }
        public int IdEtatdemande { get; set; }
        public int Idclient { get; set; }
        public DateTime Date { get; set; }
        public int Poids { get; set; }
        public int Largeur { get; set; }
        public int Hauteur { get; set; }
        public int? Notification { get; set; }

        public virtual EtatDemandeLivraison IdEtatdemandeNavigation { get; set; }
        public virtual Client IdclientNavigation { get; set; }
        public virtual ICollection<DemandeDevis> DemandeDevis { get; set; }
        public virtual ICollection<FactureClient> FactureClient { get; set; }
        public virtual ICollection<FileDemandeLivraison> FileDemandeLivraison { get; set; }
        public virtual ICollection<Offre> Offre { get; set; }
    }
}
