using BitPastry.Backend.DTO.Models.Ricette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ordini;
public class BoopProxy : BopProxy
{
    public int? IDLavorazione { get; set; }
    public int? IDOperatore { get; set; }
    public DateTime? TsPresaInCarico { get; set; }
    public DateTime? TsFineLavorazione { get; set; }
}
