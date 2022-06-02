using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string SrcOffreFile { get; set; }
        public virtual Offre IdOffreNavigation { get; set; }
    }
}
