using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class TypeCamion
    {
        public TypeCamion()
        {
            Camion = new HashSet<Camion>();
        }

        public int IdType { get; set; }
        public float? Poids { get; set; }
        public float? Largeur { get; set; }
        public float? Hauteur { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Camion> Camion { get; set; }
    }
}
