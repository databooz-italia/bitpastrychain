using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Bom
    {
        public Bom()
        {
            Datimaterialis = new HashSet<Datimateriali>();
        }

        public int IdBom { get; set; }
        public float QuantitaPeso { get; set; }
        public int IdBop { get; set; }
        public int? IdMateria { get; set; }
        public bool Deleted { get; set; }

        public virtual Bop IdBopNavigation { get; set; } = null!;
        public virtual Materieprime? IdMateriaNavigation { get; set; }
        public virtual ICollection<Datimateriali> Datimaterialis { get; set; }
    }
}
