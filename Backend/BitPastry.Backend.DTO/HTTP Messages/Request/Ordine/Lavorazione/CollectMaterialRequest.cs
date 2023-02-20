using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.HTTP_Messages.Request.Ordine.Lavorazione;
public class CollectMaterialRequest {
    [JsonProperty("QuantitàPrelevata")]
    public float? QuantitàPrelevata { get; set; }
}

