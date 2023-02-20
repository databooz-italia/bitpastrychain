using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.Core.Services;
public class OperatoriService : BaseService {
    public OperatoriService(BaseService b) : base(b) { }

    /* ----------------------------------------------------------------------------------------- */
    public IEnumerable<OperatoreProxy> Get() 
    {
        return Database
            .Operatoris
            .OrderBy(x => x.IdOperatore)
            .Where(x => x.Deleted == false)
            .Select(x => new OperatoreProxy()
            {
                ID = x.IdOperatore,
                Matricola = x.Matricola,
                Nome = x.Nome,
                Cognome = x.Cognome,
                Contatto = x.Contatto,
                TsInserimento = x.TsInserimento,
                Livello = x.Livello,
            })
            .ToList();
    }

    /* ----------------------------------------------------------------------------------------- */
    public int Create(
        int? matricola,
        string? nome,
        string? cognome,
        string? contatto,
        int? livello
    ) {
        // Controllo se i campi Nome / Cognome / Contatto sono null
        if(matricola == null || string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cognome) || string.IsNullOrEmpty(contatto))
            throw EntityException.ValueEmpty("Matricola / Nome / Cognome / Contatto sono colonne obbligatorie");

        // Controllo se questa matricola è già presente nel DB
        var isExisting = Database.Operatoris.SingleOrDefault(x => x.Matricola == matricola && x.Deleted == false);
        if (isExisting != null)
            throw EntityException.DuplicateMatricola();

        // Aggiungo l'Operatore
        var newOperatore = Database.Operatoris.Add(new Data.Operatori()
        {
            Matricola = (int)matricola,
            Nome = nome,
            Cognome = cognome,
            Contatto = contatto,
            Livello = livello ?? 0
        });
        // Aggiungo l'Operatore nella Tabella degli Account
        Database.Accounts.Add(new Data.Account() 
        { 
            Username = matricola.ToString(),
            OperatoriIdOperatore = newOperatore.Entity.IdOperatore,
            OperatoriIdOperatoreNavigation = newOperatore.Entity
        });
        Database.SaveChanges();

        return newOperatore.Entity.IdOperatore;
    }

    /* ----------------------------------------------------------------------------------------- */
    public void Delete(int id) {
        // Eliminazione Logica dell'Operatore
        var toDeleteOperatore = Database.Operatoris.SingleOrDefault(x => x.IdOperatore == id);
        if (toDeleteOperatore == null)
            throw EntityException.OperatoreNotFound();
        toDeleteOperatore.Deleted = true;

        // Eliminazione Logica dell'Account
        var toDeleteAccount = Database.Accounts.SingleOrDefault(x => x.OperatoriIdOperatore == id);
        if (toDeleteAccount != null) toDeleteAccount.Deleted = true;

        Database.SaveChanges();
    }

    /* ------------------------------------------------------------------------------------- */
    public void Update(
        int id,
        int? matricola,
        string? nome,
        string? cognome,
        string? contatto,
        int? livello
    ) {
        // Controllo se i campi Nome / Cognome / Contatto sono null
        if (matricola == null || string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cognome) || string.IsNullOrEmpty(contatto))
            throw EntityException.ValueEmpty("Matricola / Nome / Cognome / Contatto sono colonne obbligatorie");

        // Controllo che l'Operatore esista
        var toUpdate = Database.Operatoris.SingleOrDefault(x => x.IdOperatore == id);
        if (toUpdate == null)
            throw EntityException.OperatoreNotFound();

        if(toUpdate.Matricola != matricola)
        {
            // Controllo se questa matricola è già presente nel DB
            var isExisting = Database.Operatoris.SingleOrDefault(x => x.Matricola == matricola);
            if (isExisting != null)
                throw EntityException.DuplicateMatricola();
        }

        toUpdate.Matricola = (int)matricola;
        toUpdate.Nome = nome;
        toUpdate.Cognome = cognome;
        toUpdate.Contatto = contatto;
        toUpdate.Livello = livello ?? 0;

        // Aggiorno l'Account collegato all'operatore
        var account = Database.Accounts.SingleOrDefault(x => x.OperatoriIdOperatore == id);
        if (account != null)
            account.Username = matricola.ToString();

        Database.SaveChanges();
    }
}

