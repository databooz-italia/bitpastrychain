using BitPastry.Backend.DTO.HTTP_Messages.Response;
using BitPastry.Backend.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Mappers;
public class MateriaPrimaMapper 
{
    public static GetMateriaPrimaResponse Map(MateriaPrimaProxy proxy) => new GetMateriaPrimaResponse()
    {
        ID = proxy.ID,
        Nome = proxy.Nome,
        IDRicetta = proxy.IDRicetta,
        NomeRicetta = proxy.NomeRicetta,
        UnitàMisura = proxy.UnitàMisura
    };
}
