using BitPastry.Backend.Data;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ricette;
using BitPastry.Backend.DTO.Models.Ricette;

namespace BitPastry.Backend.Core.Services;
public class RicetteService : BaseService {
    public RicetteService(BaseService b) : base(b) { }

    /* ----------------------------------------------------------------------------------------- */
    public IEnumerable<RicettaProxy> Get(
        int? id, 
        bool isOnlySemiLavorati
    ) {
        var query = Database
            .Ricettes
            .OrderBy(x => x.IdRicetta)
            .Where(x => x.Deleted == false);

        // Solo una deterimanta Ricetta?
        if (id != null)
            query = query.Where(x => x.IdRicetta == id);

        // Solo i SemiLavorati?
        if (isOnlySemiLavorati)
            query = query.Where(x => x.Semilavorato == true);
         
        return query
            .Select(ricetta => new RicettaProxy()
            {
                ID = ricetta.IdRicetta,
                Nome = ricetta.Nome,
                Autore = ricetta.Autore,
                TsInserimento = ricetta.TsInserimento,
                Descrizione = ricetta.Descrizione,
                Semilavorato = ricetta.Semilavorato,
                UnitàMisura = ricetta.UnitaMisura,
                Bops = Database
                    .Bops
                    .Where(x => x.Deleted == false && x.IdRicetta == ricetta.IdRicetta)
                    .OrderBy(x => x.OrderIndex)
                    .Select(bop => new BopProxy()
                    {
                        ID = bop.IdBop,
                        OrderIndex = bop.OrderIndex,
                        Titolo = bop.Titolo,
                        Descrizione = bop.Descrizione,
                        Boms = Database
                            .Boms
                            .Where(x => x.Deleted == false && x.IdBop == bop.IdBop)
                            .Select(bom => new BomProxy()
                            {
                                ID = bom.IdBom,
                                IDMateria = bom.IdMateria ?? -1,
                                Quantità = bom.QuantitaPeso
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();
    }

    /* ----------------------------------------------------------------------------------------- */
    public int CreateOrUpdate(
        int? id,
        string? nome,
        string? autore,
        string? descrizione,
        bool semilavorato,
        string? unità,
        IEnumerable<CreateBopRequest>? bops
    ) {
        // Controllo se i campi Nome / Autore / Unità di Misura sono null
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(autore) || string.IsNullOrEmpty(unità))
            throw EntityException.ValueEmpty("Nome / Autore / Unità di Misura sono colonne obbligatorie");

        // Recupero / Creo la Ricetta
        Data.Ricette? ricetta = Database.Ricettes.SingleOrDefault(x => x.IdRicetta == id);
        if (ricetta == null) 
        {
            ricetta = Database.Ricettes.Add(new Data.Ricette()).Entity;

            // Is Semilavorato?
            ricetta.Semilavorato = semilavorato;
            if (semilavorato) // Creo la Materia Prima
                Database.Materieprimes.Add(new Data.Materieprime
                {
                    Nome = nome,
                    UnitaMisura = unità,
                    IdRicettaNavigation = ricetta
                });
        }

        // Aggiorno / Creo i vari campi
        ricetta.Autore = autore;
        ricetta.Descrizione = descrizione;
        if(ricetta.Semilavorato && (ricetta.UnitaMisura != unità || ricetta.Nome != nome))
        {
            // Aggiorno la Materia Prima associata alla Ricetta
            var isExistingMateria = Database.Materieprimes.SingleOrDefault(x => x.IdRicetta == ricetta.IdRicetta);
            if (isExistingMateria != null)
            {
                isExistingMateria.UnitaMisura = unità;
                isExistingMateria.Nome = nome;
            }
        }
        ricetta.Nome = nome;
        ricetta.UnitaMisura = unità;

        // Aggiorno / Elimino / Creo i Bops
        DoBops(ricetta, bops);

        Database.SaveChanges();
        return ricetta.IdRicetta;
    }

    private void DoBops(
        Data.Ricette ricetta,
        IEnumerable<CreateBopRequest>? bops
    ) {
        // Recupero tutti i Bops della Ricetta dal DB
        var bopsInDB = Database.Bops.Where(x => x.Deleted == false && x.IdRicetta == ricetta.IdRicetta).ToList();

        if(bops != null)
            for(int index = 0; index < bops.Count(); index++)
            {
                var bop = bops.ElementAt(index);
                // Errore di battitura da parte dell'Utente, non gestisco questo Bop
                if (string.IsNullOrEmpty(bop.Titolo))
                    continue;

                var target = bopsInDB.Where(x => x.IdBop == bop.ID).SingleOrDefault(); // Update

                // Insert
                if (target == null)
                {
                    target = new();
                    ricetta.Bops.Add(target);
                }
                else bopsInDB.Remove(target);
                
                target.OrderIndex = index;
                target.Titolo = bop.Titolo;
                target.Descrizione = bop.Descrizione;

                // Boms
                DoBoms(target, bop.Boms);
            }

        // Delete di tutto il resto
        foreach (var bop in bopsInDB)
        {
            bop.Deleted = true;

            // Delete tutte le Materie dello Step
            foreach (var bom in Database.Boms.Where(x => x.Deleted == false && x.IdBop == bop.IdBop).ToList())
                bom.Deleted = true;
        }
    }

    private void DoBoms(
        Data.Bop step,
        IEnumerable<CreateBomRequest>? boms
    ) {
        // Recupero tutte i Boms dello Step dal DB
        var bomsInDB = Database.Boms.Where(x => x.Deleted == false && x.IdBop == step.IdBop).ToList();

        if (boms != null)
            for (int index = 0; index < boms.Count(); index++)
            {
                var bom = boms.ElementAt(index);
                // Errore di battitura da parte dell'Utente, non gestisco questo Bom
                if ((bom.Quantità == null || bom.Quantità < 0) || (bom.IDMateriaPrima == null || bom.IDMateriaPrima <= -1))
                    continue;

                var target = bomsInDB.Where(x => x.IdBom == bom.ID).SingleOrDefault(); // Update

                // Insert
                if (target == null)
                {
                    target = new();
                    step.Boms.Add(target);
                }
                else bomsInDB.Remove(target);

                target.QuantitaPeso = (float)bom.Quantità;
                target.IdMateria = bom.IDMateriaPrima;
            }

        // Delete di tutto il resto
        foreach (var bom in bomsInDB)
            bom.Deleted = true;
    }

    /* ----------------------------------------------------------------------------------------- */
    public void Delete(int id) {
        //Eliminazione Logica
        var toDelete = Database.Ricettes.SingleOrDefault(x => x.IdRicetta == id);
        if (toDelete == null)
            throw EntityException.RicettaNotFound();

        toDelete.Deleted = true;

        // Delete tutti i Bops della Ricetta
        foreach (var bop in Database.Bops.Where(x => x.Deleted == false && x.IdRicetta == toDelete.IdRicetta).ToList())
        {
            bop.Deleted = true;

            // Delete tutte i Boms del Bop
            foreach (var bom in Database.Boms.Where(x => x.Deleted == false && x.IdBop == bop.IdBop).ToList())
                bom.Deleted = true;
        }

        // Delete di tutti gli Ordini aperti associati a questa Ricetta
        foreach(var ordine in Database.Ordinilavoraziones.Where(x => x.Deleted == false && x.TsFineOrdine == null && x.IdRicetta == id).ToList())
            ordine.Deleted = true;

        // Delete della Materia Prima se è un Semi Lavorato
        if(toDelete.Semilavorato)
        {
            var isExistingMateria = Database.Materieprimes.SingleOrDefault(x => x.Deleted == false && x.IdRicetta == toDelete.IdRicetta);
            if (isExistingMateria != null)
            {
                isExistingMateria.Deleted = true;

                // Eliminazione di tutti i Bops associati alla Materia Prima
                foreach (var bom in Database.Boms.Where(x => x.Deleted == false && x.IdMateria == isExistingMateria.IdMateria).ToList())
                    bom.Deleted = true;
            }
        }

        Database.SaveChanges();
    }
}

