using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Data.Queries;
public static class GetAllIngredientiCollectedQuery {
    public static IQueryable<int?> GetAllIngredientiCollected(this BitPastryDB _db, int idBop, int idLavorazione) {
        return 
            from boms in _db.Boms.Where(x => x.Deleted == false && x.IdBop == idBop)
                join materiali in _db.Datimaterialis.Where(x => x.IdLavorazione == idLavorazione)
                    on boms.IdBom equals materiali.IdBom into materiali_left_join
                from result in materiali_left_join.DefaultIfEmpty()
            select (int?)result.IdRaccoltaMateriali;
    }
}

