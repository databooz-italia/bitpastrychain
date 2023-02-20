using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Datimateriali
    {
        public int IdRaccoltaMateriali { get; set; }
        public DateTime? TsPrelievo { get; set; }
        public float QuantitaPeso { get; set; }
        public int IdLavorazione { get; set; }
        public int? IdMagazzino { get; set; }
        public int IdBom { get; set; }
        public int? IdSemilavorato { get; set; }

        public virtual Bom IdBomNavigation { get; set; } = null!;
        public virtual Consuntivilavorazione IdLavorazioneNavigation { get; set; } = null!;
        public virtual Magazzino? IdMagazzinoNavigation { get; set; }
        public virtual Semilavorati? IdSemilavoratoNavigation { get; set; }
    }
}
