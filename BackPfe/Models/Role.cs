using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class Role
    {
        public Role()
        {
            Intermediaire = new HashSet<Intermediaire>();
            RolePermission = new HashSet<RolePermission>();
        }

        public int IdRole { get; set; }
        public string Role1 { get; set; }

        public virtual ICollection<Intermediaire> Intermediaire { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
