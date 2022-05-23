using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Ville
    {
        public Ville()
        {
            TrajetIdVille1Navigation = new HashSet<Trajet>();
            TrajetIdVille2Navigation = new HashSet<Trajet>();
        }

        public int IdVille { get; set; }
        public string NomVille { get; set; }
        public int IdIntermediaire { get; set; }

        public virtual Intermediaire IdIntermediaireNavigation { get; set; }
        public virtual ICollection<Trajet> TrajetIdVille1Navigation { get; set; }
        public virtual ICollection<Trajet> TrajetIdVille2Navigation { get; set; }
    }
}
