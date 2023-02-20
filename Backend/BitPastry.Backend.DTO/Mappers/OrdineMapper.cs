using BitPastry.Backend.DTO.HTTP_Messages.Response;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine.Lavorazione;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using BitPastry.Backend.DTO.Models.Ordini;
using BitPastry.Backend.DTO.Models.Ordini.Lavorazione;
using BitPastry.Backend.DTO.Models.Ricette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Mappers;
public class OrdineMapper {

    public static GetOrdineResponse Map(OrdineProxy proxy) => new GetOrdineResponse
    {
        ID = proxy.ID,
        Quantità = proxy.Quantità,
        TsOrdine = proxy.TsOrdine,
        RifCliente = proxy.RifCliente,
        ContattoCliente = proxy.ContattoCliente,
        RicettaID = proxy.RicettaID,
        Ricetta = Map(proxy.Ricetta),
    };

    private static GetRicettaResponse Map(RicettaProxy proxy) => new GetRicettaResponse
    {
        ID = proxy.ID,
        Nome = proxy.Nome,
        Autore = proxy.Autore,
        TsInserimento = proxy.TsInserimento,
        Descrizione = proxy.Descrizione,
        Semilavorato = proxy.Semilavorato,
        UnitàMisura = proxy.UnitàMisura,
        Bops = proxy.Bops?.Select(x => Map((BoopProxy)x))
    };

    private static GetBoopsResponse Map(BoopProxy proxy) => new GetBoopsResponse
    { 
        ID = proxy.ID,
        OrderIndex = proxy.OrderIndex,
        Titolo = proxy.Titolo,
        Descrizione = proxy.Descrizione,
        IDLavorazione = proxy.IDLavorazione,
        TsPresaInCarico = proxy.TsPresaInCarico,
        TsFineLavorazione = proxy.TsFineLavorazione,
        Boms = proxy.Boms?.Select(x => Map((BoomProxy)x))
    };

    private static GetBoomResponse Map(BoomProxy proxy) => new GetBoomResponse
    { 
        ID = proxy.ID,
        IDMateria = proxy.IDMateria,
        Quantità = proxy.Quantità,
        IDRaccoltaMateriali = proxy.IDRaccoltaMateriali,
        TsPrelievo = proxy.TsPrelievo,
        QuantitàPrelevata = proxy.QuantitàPrelevata,
        MateriaPrima = MateriaPrimaMapper.Map(proxy.MateriaPrima),
    };

    public static GetCollectedMaterialResponse Map(CollectedMaterialProxy proxy) => new GetCollectedMaterialResponse
    {
        ID = proxy.ID,
        TsPrelievo = proxy.TsPrelievo,
        Quantità = proxy.Quantità
    };
}

