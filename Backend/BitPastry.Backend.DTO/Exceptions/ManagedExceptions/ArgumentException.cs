using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
public class ArgumentException
{
    public static ManagedException Illegal(string? detail = null) => new ManagedException
    {
        Title = "Illegal Argument",
        Detail = detail
    };
}