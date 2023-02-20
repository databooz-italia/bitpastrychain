using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Magazzino
    {
        public Magazzino()
        {
            Datimaterialis = new HashSet<Datimateriali>();
        }

        public string IdentificativoLottoMateriaPrima { get; set; } = null!;
        public int QuantitaColli { get; set; }
        public int UnitaPerCollo { get; set; }
        public float Peso { get; set; }
        public DateOnly DataOrdine { get; set; }
        public DateOnly DataArrivoInMagazzino { get; set; }
        public DateOnly Scadenza { get; set; }
        public DateTime? TsInizioUtilizzo { get; set; }
        public DateTime? TsFineUtilizzo { get; set; }
        public float PesoAvanzato { get; set; }
        public DateOnly DataSmaltimento { get; set; }
        public int IdMagazzino { get; set; }
        public int IdMateria { get; set; }
        public string UnitaMisura { get; set; } = null!;

        public virtual Materieprime IdMateriaNavigation { get; set; } = null!;
        public virtual ICollection<Datimateriali> Datimaterialis { get; set; }
    }
}
