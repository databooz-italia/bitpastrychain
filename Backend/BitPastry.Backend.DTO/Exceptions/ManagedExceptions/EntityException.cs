using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
public class EntityException 
{
    /* ------------------------------------------------------------------------------------- */
    public static ManagedException OperatoreNotFound() => new ManagedException
    {
        StatusCode = Status.Forbidden,
        Title = "Operatore Not Found"
    };
    public static ManagedException RicettaNotFound() => new ManagedException
    {
        StatusCode = Status.NotFound,
        Title = "Ricetta Not Found"
    };
    public static ManagedException MateriaPrimaNotFound() => new ManagedException
    {
        StatusCode = Status.NotFound,
        Title = "Materia Prima Not Found"
    };
    public static ManagedException OrdineNotFound() => new ManagedException
    {
        StatusCode = Status.NotFound,
        Title = "Ordine Not Found"
    };
    public static ManagedException LavorazioneNotFound() => new ManagedException
    {
        StatusCode = Status.NotFound,
        Title = "Lavorazione Not Found"
    };
    public static ManagedException BopNotFound() => new ManagedException
    {
        StatusCode = Status.NotFound,
        Title = "Step di Lavorazione Not Found"
    };
    public static ManagedException BomNotFound() => new ManagedException
    {
        StatusCode = Status.NotFound,
        Title = "Ingrediente Not Found"
    };

    /* ------------------------------------------------------------------------------------- */
    public static ManagedException DuplicateMatricola() => new ManagedException
    {
        Title = "Duplicate Matricola",
        Detail = "É già presente un Operatore con questa Matricola"
    };

    /* ------------------------------------------------------------------------------------- */
    public static ManagedException ValueEmpty(string? detail) => new ManagedException
    {
        Title = "Empty Value",
        Detail = detail
    };
}
