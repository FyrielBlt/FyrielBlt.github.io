using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Trajet
    {
        public int IdTrajet { get; set; }
        public int IdVille1 { get; set; }
        public int IdVille2 { get; set; }
        public int IdCamion { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Heurdepart { get; set; }

        public virtual Camion IdCamionNavigation { get; set; }
        public virtual Ville IdVille1Navigation { get; set; }
        public virtual Ville IdVille2Navigation { get; set; }
    }
}
