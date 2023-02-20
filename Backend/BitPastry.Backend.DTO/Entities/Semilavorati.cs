using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Semilavorati
    {
        public Semilavorati()
        {
            Datimaterialis = new HashSet<Datimateriali>();
        }

        public int IdSemilavorato { get; set; }
        public DateTime? TsStoccaggio { get; set; }
        public int IdLavorazione { get; set; }
        public float? QuantitaPeso { get; set; }
        public string? UnitaMisura { get; set; }

        public virtual Consuntivilavorazione IdLavorazioneNavigation { get; set; } = null!;
        public virtual ICollection<Datimateriali> Datimaterialis { get; set; }
    }
}
