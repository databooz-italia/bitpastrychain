using BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response;
public class GetOrdineResponse 
{
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("Quantità")]
    public int Quantità { get; set; }
    [JsonProperty("TsOrdine")]
    public DateTime? TsOrdine { get; set; }
    [JsonProperty("RifCliente")]
    public string RifCliente { get; set; } = null!;
    [JsonProperty("ContattoCliente")]
    public string ContattoCliente { get; set; } = null!;

    [JsonProperty("RicettaID")]
    public int RicettaID { get; set; }
    [JsonProperty("Ricetta")]
    public GetRicettaResponse Ricetta { get; set; } = null!;
}
