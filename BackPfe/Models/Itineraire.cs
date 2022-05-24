using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Itineraire
    {
        public int IdItineraire { get; set; }
        public int IdTransporteur { get; set; }
        public int IdVille { get; set; }
    }
}
