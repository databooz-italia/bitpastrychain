using BitPastry.Backend.DTO.HTTP_Messages.Response;
using BitPastry.Backend.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Mappers;
public class OperatoreMapper {

    public static GetOperatoreResponse Map(OperatoreProxy proxy) => new GetOperatoreResponse()
    {
        ID = proxy.ID,
        Matricola = proxy.Matricola,
        Nome = proxy.Nome,
        Cognome = proxy.Cognome,
        Contatto = proxy.Contatto,
        TsInserimento = proxy.TsInserimento,
        Livello = proxy.Livello,
    };

}

