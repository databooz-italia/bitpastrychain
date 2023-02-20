using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Datiprocesso
    {
        public int Idraccoltadati { get; set; }
        public int? Tipo { get; set; }
        public string? Valore { get; set; }
        public DateTime? Ts { get; set; }
        public int Idlavorazione { get; set; }
        public int IdBod { get; set; }

        public virtual Bod IdBodNavigation { get; set; } = null!;
        public virtual Consuntivilavorazione IdlavorazioneNavigation { get; set; } = null!;
    }
}
