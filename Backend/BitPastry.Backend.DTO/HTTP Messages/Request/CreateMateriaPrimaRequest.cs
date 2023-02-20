using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Request;
public class CreateMateriaPrimaRequest {
    [JsonProperty("Nome")]
    public string? Nome { get; set; }
    [JsonProperty("UnitàMisura")]
    public string? UnitàMisura { get; set; }
}