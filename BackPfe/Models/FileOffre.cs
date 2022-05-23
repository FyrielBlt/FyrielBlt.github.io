using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class FileOffre
    {
        public int IdFile { get; set; }
        public string NomFile { get; set; }
        public int IdOffre { get; set; }

        public virtual Offre IdOffreNavigation { get; set; }
    }
}
