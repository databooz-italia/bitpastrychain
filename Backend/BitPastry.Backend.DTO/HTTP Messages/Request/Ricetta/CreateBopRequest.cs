using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
public class CreateBopRequest {
    [JsonProperty("id")]
    public int? ID { get; set; }
    [JsonProperty("OrderIndex")]
    public int OrderIndex { get; set; }
    [JsonProperty("Titolo")]
    public string Titolo { get; set; } = null!;
    [JsonProperty("Descrizione")]
    public string? Descrizione { get; set; }


    [JsonProperty("Boms")]
    public IEnumerable<CreateBomRequest>? Boms { get; set; }
}
