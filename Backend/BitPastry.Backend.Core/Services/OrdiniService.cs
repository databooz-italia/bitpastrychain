using BitPastry.Backend.Data;
using BitPastry.Backend.Data.Queries;
using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.Models.Ordini;
using BitPastry.Backend.DTO.Models.Ordini.Lavorazione;
using BitPastry.Backend.DTO.Models.Ricette;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentException = BitPastry.Backend.DTO.Exceptions.ManagedExceptions.ArgumentException;

namespace BitPastry.Backend.Core.Services;
public class OrdiniService : BaseService {
    public OrdiniService(BaseService b) : base(b) { }

    #region Ordini
    /* ----------------------------------------------------------------------------------------- */
    public IEnumerable<OrdineProxy> Get(
        bool isOnlyMy,
        bool isOnlyFree
    ) {
        var query = Database
            .Ordinilavoraziones
            .OrderBy(x => x.IdOrdine)
            .Where(x => x.Deleted == false && x.TsFineOrdine == null); // Solo quelli Aperti

        // Solo l'Ordine a IO(Operatore) sto Lavorando?
        if (isOnlyMy)
            query = query.Where(x => x.IdOperatore == _auth.GetLoggedAccountId());

        // Solo gli Ordini che non sono In Carico ad Altri Operatori
        if (isOnlyFree)
            query = query.Where(x => x.IdOperatore == null);

        var ordini = query
            .Select(ordine => new OrdineProxy()
            {
                ID = ordine.IdOrdine,
                Quantità = ordine.Quantità,
                TsOrdine = ordine.TsOrdine,
                RifCliente = ordine.RifCliente,
                ContattoCliente = ordine.ContattoCliente,
                RicettaID = ordine.IdRicetta,
                Ricetta = Database
                    .Ricettes
                    .Where(x => x.IdRicetta == ordine.IdRicetta)
                    .Select(ricetta => new RicettaProxy()
                    {
                        ID = ricetta.IdRicetta,
                        Nome = ricetta.Nome,
                        Autore = ricetta.Autore,
                        TsInserimento = ricetta.TsInserimento,
                        Descrizione = ricetta.Descrizione,
                        Semilavorato = ricetta.Semilavorato,
                        UnitàMisura = ricetta.UnitaMisura
                    })
                    .Single()
            })
            .ToList();

        // Recupero i Bops dei vari Ordini
        foreach (var ordine in ordini)
            ordine.Ricetta.Bops = Database.GetBoopsInfo(ordine.Ricetta.ID, ordine.ID);
        
        return ordini;
    }

    /* ----------------------------------------------------------------------------------------- */
    public int Create(
        int? idRicetta,
        int? quantità,
        string? tsOrdine,
        string? rifCliente,
        string? contattoCliente
    ) {
        // Controllo se i campi Ricetta / Quantià / Rif. Cliente / Contatto Cliente sono vuoti
        if (idRicetta == null || (quantità == null || quantità <= 0) || string.IsNullOrEmpty(rifCliente) || string.IsNullOrEmpty(contattoCliente))
            throw EntityException.ValueEmpty("Ricetta / Quantità / Rif. Cliente / Contatto Cliente sono colonne obbligatorie");

        // Controllo se la Ricetta esiste
        var isExistingRicetta = Database.Ricettes.SingleOrDefault(x => x.IdRicetta == idRicetta);
        if (isExistingRicetta == null)
            throw EntityException.RicettaNotFound();

        // Aggiungo l'Ordine
        var newOrdine = Database.Ordinilavoraziones.Add(new Data.Ordinilavorazione()
        {
            Quantità = (int)quantità,
            TsOrdine = string.IsNullOrEmpty(tsOrdine) ? DateTime.Now : DateTime.Parse(tsOrdine),
            RifCliente = rifCliente,
            ContattoCliente = contattoCliente,
            IdRicetta = isExistingRicetta.IdRicetta,
            IdRicettaNavigation = isExistingRicetta
        });
        Database.SaveChanges();

        return newOrdine.Entity.IdOrdine;
    }

    /* ----------------------------------------------------------------------------------------- */
    public void Delete(int id) {
        // Eliminazione Logica
        var toDelete = Database.Ordinilavoraziones.SingleOrDefault(x => x.IdOrdine == id);
        if (toDelete == null)
            throw EntityException.OrdineNotFound();

        toDelete.Deleted = true;

        // Devo togliere l'Operatore da questo Ordine
        var lavorazione = Database.Consuntivilavoraziones.SingleOrDefault(x => x.TsFineLavorazione == null && x.IdOrdine == toDelete.IdOrdine);
        if (lavorazione != null)
            lavorazione.IdOperatore = null;

        Database.SaveChanges();
    }

    /* ------------------------------------------------------------------------------------- */
    public void Update(
        int id,
        int? idRicetta,
        int? quantità,
        string? tsOrdine,
        string? rifCliente,
        string? contattoCliente
    ) {
        // Controllo se i campi Ricetta / Quantià / Data / Rif. Cliente / Contatto Cliente sono vuoti
        if (idRicetta == null || (quantità == null || quantità <= 0) || string.IsNullOrEmpty(tsOrdine) || string.IsNullOrEmpty(rifCliente) || string.IsNullOrEmpty(contattoCliente))
            throw EntityException.ValueEmpty("Ricetta / Quantità / Data / Rif. Cliente / Contatto Cliente sono colonne obbligatorie");

        // Controllo che l'Ordine eisiste
        var toUpdate = Database.Ordinilavoraziones.SingleOrDefault(x => x.IdOrdine == id);
        if (toUpdate == null)
            throw EntityException.OrdineNotFound();

        // Controllo se la Ricetta esiste
        var isExistingRicetta = Database.Ricettes.SingleOrDefault(x => x.IdRicetta == idRicetta);
        if (isExistingRicetta == null)
            throw EntityException.RicettaNotFound();

        toUpdate.IdRicetta = isExistingRicetta.IdRicetta;
        if(toUpdate.IdRicetta != isExistingRicetta.IdRicetta)
        {
            // Devo togliere l'Operatore da questo Ordine
            var lavorazione = Database.Consuntivilavoraziones.SingleOrDefault(x => x.TsFineLavorazione == null && x.IdOrdine == toUpdate.IdOrdine);
            if (lavorazione != null)
                lavorazione.IdOperatore = null;
        }
        toUpdate.Quantità = (int)quantità;
        toUpdate.TsOrdine = DateTime.Parse(tsOrdine);
        toUpdate.RifCliente = rifCliente;
        toUpdate.ContattoCliente = contattoCliente;

        Database.SaveChanges();
    }
    #endregion

    #region Lavorazioni
    /* ------------------------------------------------------------------------------------- */
    public void Completee() {
        // Recupero l'Ordine a cui sta Lavorando l'Operatore
        int idOperatore = _auth.GetLoggedAccountId();
        var ordine = Database.GetOrdine(idOperatore);

        // Controllo che tutti Steps di Lavorazione siano Finiti
        var isAllMaterialCollected = Database.GetAllStepsCompleted(ordine.IdRicetta, ordine.IdOrdine)?.Contains(null);
        if (isAllMaterialCollected != null && isAllMaterialCollected == true)
            throw PresaCaricoException.NotAllStepsCompleted();

        // Aggiorno il TS Fine Ordine e Rimuovo L'Operatore
        ordine.TsFineOrdine = DateTime.Now;
        ordine.IdOperatore = null;
        Database.SaveChanges();
    }

    public void Completee(int idBop) {
        // Recupero l'Ordine a cui sta Lavorando l'Operatore
        int idOperatore = _auth.GetLoggedAccountId();
        var ordine = Database.GetOrdine(idOperatore);

        // Recupero o creo la Lavorazione
        var isExistingLavorazione = Database.GetOrCreateLavorazione(ordine.IdOrdine, idBop, idOperatore);

        // Controllo che tutti gli Ingredienti siano stati Raccolti
        var isAllMaterialCollected = Database.GetAllIngredientiCollected(isExistingLavorazione.IdBop, isExistingLavorazione.IdLavorazione)?.Contains(null);
        if (isAllMaterialCollected != null && isAllMaterialCollected == true)
            throw PresaCaricoException.NotAllMaterialCollected();

        // Aggiorno il TS Fine Lavorazione
        isExistingLavorazione.TsFineLavorazione = DateTime.Now;
        Database.SaveChanges();
    }

    /* ------------------------------------------------------------------------------------- */
    public CollectedMaterialProxy Completee(
        string? dirtIdBop,
        string? dirtIdBom,
        float? quantitàPrelevata
    ) {
        // Pulisco gli ID
        if(string.IsNullOrEmpty(dirtIdBop) || string.IsNullOrEmpty(dirtIdBom))
            throw ArgumentException.Illegal();
        var idBop = int.Parse(dirtIdBop);
        var idBom = int.Parse(dirtIdBom);

        // Recupero l'Ordine a cui sta Lavorando l'Operatore
        int idOperatore = _auth.GetLoggedAccountId();
        var ordine = Database.GetOrdine(idOperatore);

        // Controllo se il Bom esiste
        var isExistingBom = Database.Boms.SingleOrDefault(x => x.Deleted == false && x.IdBom == idBom);
        if (isExistingBom == null)
            throw EntityException.BomNotFound();

        // Controllo che la quantità Prelevata sia maggiore o uguale a quella Attesa
        if (quantitàPrelevata == null)
            throw ArgumentException.Illegal("Inserire la quantità.");

        // Recupero o creo la Lavorazione
        var isExistingLavorazione = Database.GetOrCreateLavorazione(ordine.IdOrdine, idBop, idOperatore);

        // Recupero o creo il CollectedMaterial
        var isExistingRaccolta = Database.Datimaterialis.SingleOrDefault(x => x.IdBom == isExistingBom.IdBom && x.IdLavorazione == isExistingLavorazione.IdLavorazione);
        if(isExistingRaccolta == null)
        {
            // Prelevo il Bom
            isExistingRaccolta = Database.Datimaterialis.Add(new Data.Datimateriali
            {
                TsPrelievo = DateTime.Now,
                QuantitaPeso = (float)quantitàPrelevata,
                IdLavorazione = isExistingLavorazione.IdLavorazione,
                IdBom = isExistingBom.IdBom
            }).Entity;
            Database.SaveChanges();
        }
        
        return new CollectedMaterialProxy
        { 
            ID = isExistingRaccolta.IdRaccoltaMateriali,
            TsPrelievo = isExistingRaccolta.TsPrelievo,
            Quantità = isExistingRaccolta.QuantitaPeso
        };
    }

    #endregion
}

