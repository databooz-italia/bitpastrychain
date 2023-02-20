using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Bod
    {
        public Bod()
        {
            Datiprocessos = new HashSet<Datiprocesso>();
        }

        public int IdBod { get; set; }
        public string? Descrizione { get; set; }
        public string? UnitaMisura { get; set; }
        public int IdBop { get; set; }
        public sbyte? Obbligatorio { get; set; }

        public virtual Bop IdBopNavigation { get; set; } = null!;
        public virtual ICollection<Datiprocesso> Datiprocessos { get; set; }
    }
}
