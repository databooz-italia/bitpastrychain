using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using BitPastry.Backend.DTO.Models.Ricette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Mappers;
public class RicettaMapper {

    public static GetRicettaResponse Map(RicettaProxy proxy) => new GetRicettaResponse()
    {
        ID = proxy.ID,
        Nome = proxy.Nome,
        Autore = proxy.Autore,
        TsInserimento = proxy.TsInserimento,
        Descrizione = proxy.Descrizione,
        Semilavorato = proxy.Semilavorato,
        UnitàMisura = proxy.UnitàMisura,
        Bops = proxy.Bops?.Select(Map)
    };

    public static GetBopResponse Map(BopProxy proxy) => new GetBopResponse()
    {
        ID = proxy.ID,
        OrderIndex = proxy.OrderIndex,
        Titolo = proxy.Titolo,
        Descrizione = proxy.Descrizione,
        Boms = proxy.Boms?.Select(Map)
    };

    public static GetBomResponse Map(BomProxy proxy) => new GetBomResponse()
    {
        ID = proxy.ID,
        IDMateria = proxy.IDMateria,
        Quantità = proxy.Quantità
    };
}
