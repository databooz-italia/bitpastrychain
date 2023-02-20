using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Models.Ricette;
public class BomProxy {
    public int ID { get; set; }
    public int IDMateria { get; set; }
    public float Quantità { get; set; }
}
