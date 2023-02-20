using BitPastry.Backend.DTO.Models;
using BitPastry.Backend.DTO.Models.Ordini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Data.Queries;
public static class GetBoomInfoQuery {
    public static IEnumerable<BoomProxy> GetBoomsInfo(this BitPastryDB _db, int idBop, int? idLavorazione) {
        return
            from bom in _db.Boms.Where(x => x.Deleted == false && x.IdBop == idBop)
            join raccolta in _db.Datimaterialis.Where(x => x.IdLavorazione == idLavorazione)
                on bom.IdBom equals raccolta.IdBom into raccolta_left_join
            from result in raccolta_left_join.DefaultIfEmpty()
            select new BoomProxy
            {
                ID = bom.IdBom,
                IDMateria = bom.IdMateria ?? -1,
                Quantità = bom.QuantitaPeso,
                IDRaccoltaMateriali = result.IdRaccoltaMateriali,
                TsPrelievo = result.TsPrelievo,
                QuantitàPrelevata = result.QuantitaPeso,
                MateriaPrima = _db
                    .Materieprimes
                    .Where(x => x.IdMateria == bom.IdMateria)
                    .Select(materiaPrima => new MateriaPrimaProxy
                    {
                        ID = materiaPrima.IdMateria,
                        Nome = materiaPrima.Nome,
                        UnitàMisura = materiaPrima.UnitaMisura
                    })
                    .Single(),
            };
    }
}
