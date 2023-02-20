using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ordini.Lavorazione;
public class CollectedMaterialProxy {
    public int ID { get; set; }
    public DateTime? TsPrelievo { get; set; }
    public float Quantità { get; set; }
}
