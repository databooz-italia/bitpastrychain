using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using BitPastry.Backend.DTO.Models;
using BitPastry.Backend.DTO.Models.Ordini;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine;
public class GetBoomResponse : GetBomResponse {
    [JsonProperty("MateriaPrima")]
    public GetMateriaPrimaResponse MateriaPrima { get; set; } = null!;

    [JsonProperty("IDRaccoltaMateriali")]
    public int? IDRaccoltaMateriali { get; set; }
    [JsonProperty("TsPrelievo")]
    public DateTime? TsPrelievo { get; set; }
    [JsonProperty("QuantitàPrelevata")]
    public float? QuantitàPrelevata { get; set; }
}
