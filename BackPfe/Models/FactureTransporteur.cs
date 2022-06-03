using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class FactureTransporteur
    {
        public int IdFactTransporteur { get; set; }
        public int IdOffre { get; set; }
        public string FactureFile { get; set; }
        public string PayementFile { get; set; }
        public int? Notification { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string SrcFactureFile { get; set; }
        [NotMapped]
        public string SrcPayementFile { get; set; }
        public virtual Offre IdOffreNavigation { get; set; }
    }
}
