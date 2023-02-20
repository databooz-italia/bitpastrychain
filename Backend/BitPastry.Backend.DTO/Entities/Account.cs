using System;
using System.Collections.Generic;

namespace BitPastry.Backend.Data
{
    public partial class Account
    {
        public int IdAccount { get; set; }
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public DateTime? TsCreazione { get; set; }
        public bool Deleted { get; set; }
        public int? OperatoriIdOperatore { get; set; }

        public virtual Operatori? OperatoriIdOperatoreNavigation { get; set; }
    }
}
