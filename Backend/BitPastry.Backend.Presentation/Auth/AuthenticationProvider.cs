using BitPastry.Backend.Core.Interfaces;
using BitPastry.Backend.Data;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace BitPastry.Backend.Presentation.Auth;
public class AuthenticationProvider : HttpContextAccessor, IAuthenticationProvider
{        
    public AuthenticationProvider() { }

    /* ----------------------------------------------------------------------------------------- */
    public int GetLoggedAccountId()
    {
        var operatoreID = this.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

        // Controllo che l'ID non sia null
        if (operatoreID == null)
            throw AuthException.OperatoreNotFound();

        return Int32.Parse(operatoreID);
    }

    /* ----------------------------------------------------------------------------------------- */
    public string GetMachineID()
    {
        return "Machine Info";
    }
}