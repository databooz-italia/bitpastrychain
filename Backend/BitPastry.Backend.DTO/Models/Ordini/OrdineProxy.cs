using BitPastry.Backend.DTO.Models.Ricette;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ordini;
public class OrdineProxy
{
    public int ID { get; set; }
    public int Quantità { get; set; }
    public DateTime? TsOrdine { get; set; }
    public string RifCliente { get; set; } = null!;
    public string ContattoCliente { get; set; } = null!;

    public int RicettaID { get; set; }
    public RicettaProxy Ricetta { get; set; } = null!;
}
