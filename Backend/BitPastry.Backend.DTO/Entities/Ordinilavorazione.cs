using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Ordinilavorazione
    {
        public Ordinilavorazione()
        {
            Consuntivilavoraziones = new HashSet<Consuntivilavorazione>();
        }

        public int IdOrdine { get; set; }
        public int Quantità { get; set; }
        public int IdDestinatarioCliente { get; set; }
        public int IdRicetta { get; set; }
        public string RifCliente { get; set; } = null!;
        public string ContattoCliente { get; set; } = null!;
        public DateTime? TsFineOrdine { get; set; }
        public bool Deleted { get; set; }
        public DateTime? TsOrdine { get; set; }
        public int? IdOperatore { get; set; }

        public virtual Operatori? IdOperatoreNavigation { get; set; }
        public virtual Ricette IdRicettaNavigation { get; set; } = null!;
        public virtual ICollection<Consuntivilavorazione> Consuntivilavoraziones { get; set; }
    }
}
