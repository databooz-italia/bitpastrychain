using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
public class GetBopResponse {
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("OrderIndex")]
    public int OrderIndex { get; set; }
    [JsonProperty("Titolo")]
    public string Titolo { get; set; }
    [JsonProperty("Descrizione")]
    public string? Descrizione { get; set; }

    [JsonProperty("Boms")]
    public IEnumerable<GetBomResponse>? Boms { get; set; }
}
