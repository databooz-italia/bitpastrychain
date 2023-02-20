using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine;
public class GetBoopsResponse: GetBopResponse {
    [JsonProperty("IDLavorazione")]
    public int? IDLavorazione { get; set; }
    [JsonProperty("TsPresaInCarico")]
    public DateTime? TsPresaInCarico { get; set; }
    [JsonProperty("TsFineLavorazione")]
    public DateTime? TsFineLavorazione { get; set; }
}
