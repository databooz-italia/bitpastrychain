using BitPastry.Backend.Data.Queries;
using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.Models;
using BitPastry.Backend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Core.Services;
public class PresaCaricoService : BaseService {
    public PresaCaricoService(BaseService b) : base(b) { }

    /* ----------------------------------------------------------------------------------------- */
    public bool Check()
    {
        int idOperatore = _auth.GetLoggedAccountId();

        // Controllo che l'Operatore esista
        var isOperatoreExisting = Database.Operatoris.SingleOrDefault(x => x.Deleted == false && x.IdOperatore == idOperatore);
        if (isOperatoreExisting == null)
            throw EntityException.OperatoreNotFound();
        
        // Controllo se l'Utente è associato ad un Ordine
        var ordini = Database.Ordinilavoraziones.Where(x => x.Deleted == false && x.TsFineOrdine == null && x.IdOperatore == idOperatore).ToList();

        // Controllo se per sbaglio l'Utente è associato a più Ordini, in questo caso libero tutti gli Ordini tranne 1
        if(ordini.Count > 1)
        {
            for (int i = 0; i < ordini.Count - 1; i++)
                ordini.ElementAt(i).IdOperatore = null;
            Database.SaveChanges();
        }
        
        return ordini.Count != 0;
    }

    /* ----------------------------------------------------------------------------------------- */
    public void Pick(int idOrdine) 
    {
        // Controllo che l'Utente non abbia già qualcosa in Sospeso
        if (Check())
            throw PresaCaricoException.Pending();

        int idOperatore = _auth.GetLoggedAccountId();

        // Controllo che l'Operatore esista
        var isOperatoreExisting = Database.Operatoris.SingleOrDefault(x => x.Deleted == false && x.IdOperatore == idOperatore);
        if (isOperatoreExisting == null)
            throw EntityException.OperatoreNotFound();

        // Controllo che l'Ordine esista
        var isOrdineExisting = Database.Ordinilavoraziones.SingleOrDefault(x => x.Deleted == false && x.TsFineOrdine == null && x.IdOperatore == null && x.IdOrdine == idOrdine);
        if (isOrdineExisting == null)
            throw EntityException.OrdineNotFound();

        // Assegno l'Ordine all'Operatore
        isOrdineExisting.IdOperatore = idOperatore;
        Database.SaveChanges();
    }

    /* ----------------------------------------------------------------------------------------- */
    public void Release()
    {
        int idOperatore = _auth.GetLoggedAccountId();

        // Controllo che l'Operatore esista
        var isOperatoreExisting = Database.Operatoris.SingleOrDefault(x => x.Deleted == false && x.IdOperatore == idOperatore);
        if (isOperatoreExisting == null)
            throw EntityException.OperatoreNotFound();

        // Rimuovo l'Operatore da tutti gli Ordini
        var ordini = Database.Ordinilavoraziones.Where(x => x.Deleted == false && x.TsFineOrdine == null && x.IdOperatore == idOperatore).ToList();
        for (int i = 0; i < ordini.Count; i++)
            ordini.ElementAt(i).IdOperatore = null;
        Database.SaveChanges();
    }
}

