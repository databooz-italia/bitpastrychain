using BitPastry.Backend.Data;
using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Core.Services;
public class MateriePrimeService : BaseService {
    public MateriePrimeService(BaseService b) : base(b) { }

    /* ----------------------------------------------------------------------------------------- */
    public IEnumerable<MateriaPrimaProxy> Get() 
    {
        return Database
            .Materieprimes
            .OrderBy(x => x.IdMateria)
            .Where(x => x.Deleted == false)
            .Select(materiaPrima => new MateriaPrimaProxy()
            {
                ID = materiaPrima.IdMateria,
                Nome = materiaPrima.Nome,
                IDRicetta = materiaPrima.IdRicetta,
                NomeRicetta = materiaPrima.IdRicetta != null ? Database.Ricettes.First(x => x.IdRicetta == materiaPrima.IdRicetta).Nome : null,
                UnitàMisura = materiaPrima.UnitaMisura
            })
            .ToList();
    }

    /* ----------------------------------------------------------------------------------------- */
    public int Create(
        string? nome,
        string? unitàMisura
    ) {
        // Controllo se il campo Nome non è null
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(unitàMisura))
            throw EntityException.ValueEmpty("Nome / Unità di Misura sono colonne obbligatorie");

        // Aggiungo la Materia Prima
        var newOrdine = Database.Materieprimes.Add(new Data.Materieprime
        {
            Nome = nome,
            UnitaMisura = unitàMisura
        });

        Database.SaveChanges();
        return newOrdine.Entity.IdMateria;
    }

    /* ----------------------------------------------------------------------------------------- */
    public void Delete(int id) {
        // Eliminazione Logica
        var toDelete = Database.Materieprimes.SingleOrDefault(x => x.IdMateria == id);
        if (toDelete == null)
            throw EntityException.MateriaPrimaNotFound();

        toDelete.Deleted = true;

        // Eliminazione di tutti i Bops associati alla Materia Prima
        foreach (var bom in Database.Boms.Where(x => x.Deleted == false && x.IdMateria == toDelete.IdMateria).ToList())
            bom.Deleted = true;

        Database.SaveChanges();
    }

    /* ------------------------------------------------------------------------------------- */
    public void Update(
        int id,
        string? nome,
        string? unitàMisura
    ) {
        // Controllo che la Materia prima esista
        var toUpdate = Database.Materieprimes.SingleOrDefault(x => x.IdMateria == id);
        if (toUpdate == null)
            throw EntityException.MateriaPrimaNotFound();

        // Controllo se il campo Nome non è null
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(unitàMisura))
            throw EntityException.ValueEmpty("Nome / Unità di Misura sono colonne obbligatorie");

        toUpdate.Nome = nome;
        toUpdate.UnitaMisura = unitàMisura;

        Database.SaveChanges();
    }
}

