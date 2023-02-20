using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Materieprime
    {
        public Materieprime()
        {
            Boms = new HashSet<Bom>();
            Magazzinos = new HashSet<Magazzino>();
        }

        public int IdMateria { get; set; }
        public string Nome { get; set; } = null!;
        public int? IdRicetta { get; set; }
        public string UnitaMisura { get; set; } = null!;
        public bool Deleted { get; set; }

        public virtual Ricette? IdRicettaNavigation { get; set; }
        public virtual ICollection<Bom> Boms { get; set; }
        public virtual ICollection<Magazzino> Magazzinos { get; set; }
    }
}
