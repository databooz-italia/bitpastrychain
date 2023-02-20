using BitPastry.Backend.DTO.Models.Ordini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Data.Queries;
public static class GetOrCreateLavorazioneQuery {
    public static Consuntivilavorazione GetOrCreateLavorazione(this BitPastryDB _db, int idOrine, int idBop, int idOperatore) {
        var target = _db.Consuntivilavoraziones.SingleOrDefault(x => x.IdOrdine == idOrine && x.IdBop == idBop);

        // Se non esiste lo Creo
        if (target == null)
        {
            target = _db.Consuntivilavoraziones.Add(new Consuntivilavorazione
            {
                TsPresaInCarico = DateTime.Now,
                IdOrdine = idOrine,
                IdBop = idBop,
                IdOperatore = idOperatore
            }).Entity;
            _db.SaveChanges();
        }
        return target;
    }
}
