using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Request.Ordine;
public class CreateOrdineRequest {
    [JsonProperty("Quantità")]
    public int? Quantità { get; set; }
    [JsonProperty("TsOrdine")]
    public string? TsOrdine { get; set; }
    [JsonProperty("RifCliente")]
    public string? RifCliente { get; set; } = null!;
    [JsonProperty("ContattoCliente")]
    public string? ContattoCliente { get; set; } = null!;

    [JsonProperty("RicettaID")]
    public int? RicettaID { get; set; }
}

