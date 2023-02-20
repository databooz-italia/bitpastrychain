using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Request;
public class CreateRicettaRequest {
    [JsonProperty("id")]
    public int? ID { get; set; }
    [JsonProperty("Nome")]
    public string? Nome { get; set; }
    [JsonProperty("Autore")]
    public string? Autore { get; set; }
    [JsonProperty("Descrizione")]
    public string? Descrizione { get; set; }
    [JsonProperty("IsSemiLavorato")]
    public bool Semilavorato { get; set; }
    [JsonProperty("UnitàMisura")]
    public string? Unità { get; set; }


    [JsonProperty("Bops")]
    public IEnumerable<CreateBopRequest>? Bops { get; set; }
}
