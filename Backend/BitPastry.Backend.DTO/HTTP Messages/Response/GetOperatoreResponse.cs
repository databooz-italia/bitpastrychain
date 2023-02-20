using BitPastry.Backend.Data;
using BitPastry.Backend.DTO.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response;
public class GetOperatoreResponse {
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("Matricola")]
    public int Matricola { get; set; }
    [JsonProperty("Nome")]
    public string Nome { get; set; } = null!;
    [JsonProperty("Cognome")]
    public string Cognome { get; set; } = null!;
    [JsonProperty("Contatto")]
    public string Contatto { get; set; } = null!;
    [JsonProperty("DataInserimento")]
    public DateTime? TsInserimento { get; set; }
    [JsonProperty("LivelloTitolo")]
    public int Livello { get; set; }
}
