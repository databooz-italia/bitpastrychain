using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models;
public class OperatoreProxy {
    public int ID { get; set; }
    public int Matricola { get; set; }
    public string Nome { get; set; } = null!;
    public string Cognome { get; set; } = null!;
    public string? Contatto { get; set; }
    public DateTime? TsInserimento { get; set; }
    public int Livello { get; set; }
}

