using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models;
public class MateriaPrimaProxy {
    public int ID { get; set; }
    public string Nome { get; set; } = null!;

    public int? IDRicetta { get; set; }
    public string? NomeRicetta { get; set; }
    public string UnitàMisura { get; set; } = null!;
}