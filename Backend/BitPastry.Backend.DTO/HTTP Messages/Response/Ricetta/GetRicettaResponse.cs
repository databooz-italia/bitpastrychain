using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
public class GetRicettaResponse {
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("Nome")]
    public string Nome { get; set; } = null!;
    [JsonProperty("Autore")]
    public string Autore { get; set; } = null!;
    [JsonProperty("DataInserimento")]
    public DateTime? TsInserimento { get; set; }
    [JsonProperty("Descrizione")]
    public string? Descrizione { get; set; }
    [JsonProperty("IsSemiLavorato")]
    public bool Semilavorato { get; set; }
    [JsonProperty("UnitàMisura")]
    public string UnitàMisura { get; set; } = null!;

    [JsonProperty("Bops")]
    public IEnumerable<GetBopResponse>? Bops { get; set; }
}