using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Consuntivilavorazione
    {
        public Consuntivilavorazione()
        {
            Datimaterialis = new HashSet<Datimateriali>();
            Datiprocessos = new HashSet<Datiprocesso>();
            Lottiproduziones = new HashSet<Lottiproduzione>();
            Semilavoratis = new HashSet<Semilavorati>();
        }

        public float QuantitaPeso { get; set; }
        public DateTime? TsPresaInCarico { get; set; }
        public DateTime? TsInizioLavorazione { get; set; }
        public DateTime? TsFineLavorazione { get; set; }
        public int IdLavorazione { get; set; }
        public int IdOrdine { get; set; }
        public int IdBop { get; set; }
        public int? IdOperatore { get; set; }
        public string? UnitaMisura { get; set; }

        public virtual Bop IdBopNavigation { get; set; } = null!;
        public virtual Operatori? IdOperatoreNavigation { get; set; }
        public virtual Ordinilavorazione IdOrdineNavigation { get; set; } = null!;
        public virtual ICollection<Datimateriali> Datimaterialis { get; set; }
        public virtual ICollection<Datiprocesso> Datiprocessos { get; set; }
        public virtual ICollection<Lottiproduzione> Lottiproduziones { get; set; }
        public virtual ICollection<Semilavorati> Semilavoratis { get; set; }
    }
}
