using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
public class CreateBomRequest {
    [JsonProperty("id")]
    public int? ID { get; set; }
    [JsonProperty("IDMateriaPrima")]
    public int? IDMateriaPrima { get; set; }
    [JsonProperty("Quantità")]
    public float? Quantità { get; set; }
}
