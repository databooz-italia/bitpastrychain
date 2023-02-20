using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
public class GetBomResponse {
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("IDMateriaPrima")]
    public int IDMateria { get; set; }
    [JsonProperty("Quantità")]
    public float Quantità { get; set; }
}
