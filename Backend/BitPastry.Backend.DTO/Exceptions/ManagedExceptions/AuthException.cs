using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
public class AuthException
{
    public static ManagedException UsernameEmpty() => new ManagedException
    {
        Title = "Username empty"
    };

    public static ManagedException AuthFail() => new ManagedException
    {
        Title = "Invalid Username or Password"
    };

    public static ManagedException OperatoreNotFound() => new ManagedException
    {
        StatusCode = Status.Forbidden,
        Title = "Operatore non trovato",
    };
}
