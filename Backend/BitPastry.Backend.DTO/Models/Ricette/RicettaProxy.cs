using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ricette;
public class RicettaProxy
{
    public int ID { get; set; }
    public string Nome { get; set; } = null!;
    public string Autore { get; set; } = null!;
    public DateTime? TsInserimento { get; set; }
    public string? Descrizione { get; set; }
    public bool Semilavorato { get; set; }
    public string UnitàMisura { get; set; } = null!;

    public IEnumerable<BopProxy>? Bops { get; set; }
}