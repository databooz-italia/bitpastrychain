using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine.Lavorazione;
public class GetCollectedMaterialResponse {
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("TsPrelievo")]
    public DateTime? TsPrelievo { get; set; }
    [JsonProperty("Quantità")]
    public float Quantità { get; set; }
}
