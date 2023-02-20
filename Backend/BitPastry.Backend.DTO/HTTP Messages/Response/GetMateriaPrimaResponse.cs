using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response;
public class GetMateriaPrimaResponse {
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("Nome")]
    public string Nome { get; set; } = null!;
    [JsonProperty("UnitàMisura")]
    public string UnitàMisura { get; set; } = null!;
    [JsonProperty("IDRicetta")]
    public int? IDRicetta { get; set; }
    [JsonProperty("NomeRicetta")]
    public string? NomeRicetta { get; set; }
}
