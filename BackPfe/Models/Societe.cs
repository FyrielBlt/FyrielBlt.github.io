using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Societe
    {
        public Societe()
        {
            Users = new HashSet<Users>();
        }

        public int IdSociete { get; set; }
        public string Nom { get; set; }
        public string Adress { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
