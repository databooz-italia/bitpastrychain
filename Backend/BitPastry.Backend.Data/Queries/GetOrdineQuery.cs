using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Data.Queries;
public static class GetOrdineQuery {
    public static Ordinilavorazione GetOrdine(this BitPastryDB _db, int idOperatore) {
        // Controllo che l'Operatore esista
        var isOperatoreExisting = _db.Operatoris.SingleOrDefault(x => x.Deleted == false && x.IdOperatore == idOperatore);
        if (isOperatoreExisting == null)
            throw EntityException.OperatoreNotFound();

        // Recupero l'Ordine che sta Preparando l'Operatore
        var target = _db.Ordinilavoraziones.SingleOrDefault(x => x.Deleted == false && x.TsFineOrdine == null && x.IdOperatore == idOperatore);
        if (target == null)
            throw EntityException.OrdineNotFound();
        return target;
    }
}
