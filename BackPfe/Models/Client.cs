﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Client
    {
        public Client()
        {
            DemandeLivraison = new HashSet<DemandeLivraison>();
        }

        public int Idclient { get; set; }
        public int Iduser { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }


        public virtual Users IduserNavigation { get; set; }
        public virtual ICollection<DemandeLivraison> DemandeLivraison { get; set; }
    }
}
