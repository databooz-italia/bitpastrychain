using BitPastry.Backend.DTO.Models.Ordini;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BitPastry.Backend.Data.Queries.GetBoopsInfoQuery;

namespace BitPastry.Backend.Data.Queries;
public static class GetBoopsInfoQuery {
    public static IEnumerable<BoopProxy> GetBoopsInfo(this BitPastryDB _db, int idRicetta, int idOrdine) {
        var boops = (
            from bop in _db.Bops.Where(x => x.IdRicetta == idRicetta && x.Deleted == false)
            join lavorazioni in _db.Consuntivilavoraziones.Where(x => x.IdOrdine == idOrdine)
                on bop.IdBop equals lavorazioni.IdBop into bops_left_join
            from result in bops_left_join.DefaultIfEmpty()
            orderby bop.OrderIndex
            select new BoopProxy
            {
                ID = bop.IdBop,
                OrderIndex = bop.OrderIndex,
                Titolo = bop.Titolo,
                Descrizione = bop.Descrizione,
                IDLavorazione = result.IdLavorazione,
                IDOperatore = result.IdOperatore,
                TsPresaInCarico = result.TsPresaInCarico,
                TsFineLavorazione = result.TsFineLavorazione,
                // Boms
            }
        ).
        ToList();

        // Recupero tutte le Booms associate al Boop
        foreach (var boop in boops)
            boop.Boms = _db.GetBoomsInfo(boop.ID, boop.IDLavorazione);

        return boops;
    }
}
