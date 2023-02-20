using BitPastry.Backend.DTO.Models.Ricette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ordini;
public class BoomProxy : BomProxy 
{
    public MateriaPrimaProxy MateriaPrima { get; set; } = null!;

    public int? IDRaccoltaMateriali { get; set; }
    public DateTime? TsPrelievo { get; set; }
    public float? QuantitàPrelevata { get; set; }
}
