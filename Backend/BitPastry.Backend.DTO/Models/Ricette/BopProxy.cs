using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ricette;
public class BopProxy {
    public int ID { get; set; }
    public int OrderIndex { get; set; }
    public string Titolo { get; set; } = null!;
    public string? Descrizione { get; set; }


    public IEnumerable<BomProxy>? Boms { get; set; }
}
