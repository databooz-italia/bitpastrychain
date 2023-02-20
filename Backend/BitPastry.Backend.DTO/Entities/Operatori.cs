using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Operatori
    {
        public Operatori()
        {
            Accounts = new HashSet<Account>();
            Consuntivilavoraziones = new HashSet<Consuntivilavorazione>();
            Ordinilavoraziones = new HashSet<Ordinilavorazione>();
        }

        public int IdOperatore { get; set; }
        public int Matricola { get; set; }
        public string Nome { get; set; } = null!;
        public string Cognome { get; set; } = null!;
        public int Livello { get; set; }
        public string? Contatto { get; set; }
        public DateTime? TsInserimento { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Consuntivilavorazione> Consuntivilavoraziones { get; set; }
        public virtual ICollection<Ordinilavorazione> Ordinilavoraziones { get; set; }
    }
}
