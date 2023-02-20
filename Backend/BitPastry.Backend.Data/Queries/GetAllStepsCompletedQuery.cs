using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Data.Queries;
public static class GetAllStepsCompletedQuery {
    public static IQueryable<int?> GetAllStepsCompleted(this BitPastryDB _db, int idRicetta, int idOrdine) {
        return
            from bops in _db.Bops.Where(x => x.Deleted == false && x.IdRicetta == idRicetta)
                join lavorazioni in _db.Consuntivilavoraziones.Where(x => x.IdOrdine == idOrdine)
                    on bops.IdBop equals lavorazioni.IdBop into lavorazioni_left_join
            from result in lavorazioni_left_join.DefaultIfEmpty()
            select (int?)result.IdLavorazione;
    }
}
