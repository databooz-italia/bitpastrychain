using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Request.Operatore;
public class CreateOperatoreRequest {
    [JsonProperty("Matricola")]
    public int? Matricola { get; set; }
    [JsonProperty("Nome")]
    public string? Nome { get; set; }
    [JsonProperty("Cognome")]
    public string? Cognome { get; set; }
    [JsonProperty("Contatto")]
    public string? Contatto { get; set; }
    [JsonProperty("Livello")]
    public int? Livello { get; set; }
}

