using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Ricette
    {
        public Ricette()
        {
            Bops = new HashSet<Bop>();
            Materieprimes = new HashSet<Materieprime>();
            Ordinilavoraziones = new HashSet<Ordinilavorazione>();
        }

        public int IdRicetta { get; set; }
        public string Nome { get; set; } = null!;
        public string Autore { get; set; } = null!;
        public string? Descrizione { get; set; }
        public bool Semilavorato { get; set; }
        public string UnitaMisura { get; set; } = null!;
        public DateTime? TsInserimento { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Bop> Bops { get; set; }
        public virtual ICollection<Materieprime> Materieprimes { get; set; }
        public virtual ICollection<Ordinilavorazione> Ordinilavoraziones { get; set; }
    }
}
