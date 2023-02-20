using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Bop
    {
        public Bop()
        {
            Bods = new HashSet<Bod>();
            Boms = new HashSet<Bom>();
            Consuntivilavoraziones = new HashSet<Consuntivilavorazione>();
        }

        public int IdBop { get; set; }
        public int OrderIndex { get; set; }
        public string Titolo { get; set; } = null!;
        public string? Descrizione { get; set; }
        public int IdRicetta { get; set; }
        public bool Deleted { get; set; }

        public virtual Ricette IdRicettaNavigation { get; set; } = null!;
        public virtual ICollection<Bod> Bods { get; set; }
        public virtual ICollection<Bom> Boms { get; set; }
        public virtual ICollection<Consuntivilavorazione> Consuntivilavoraziones { get; set; }
    }
}
