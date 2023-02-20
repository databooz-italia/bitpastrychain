using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Lottiproduzione
    {
        public int IdLotto { get; set; }
        public int Quantità { get; set; }
        public DateTime? TsConfezionamento { get; set; }
        public int IdLavorazione { get; set; }
        public string? CodiceInterno { get; set; }

        public virtual Consuntivilavorazione IdLavorazioneNavigation { get; set; } = null!;
    }
}
