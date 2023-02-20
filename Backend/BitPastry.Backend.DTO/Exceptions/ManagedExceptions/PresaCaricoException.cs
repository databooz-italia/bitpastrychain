using BitPastry.Backend.DTO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
public class PresaCaricoException
{
    public static ManagedException FailedPick() => new ManagedException
    {
        Title = "Impossible to Pick order"
    };

    public static ManagedException FailedRelease() => new ManagedException
    {
        Title = "Impossible to Release order"
    };

    public static ManagedException Pending() => new ManagedException
    {
        Title = "The Operator has another Pending order"
    };

    public static ManagedException NotAllMaterialCollected() => new ManagedException
    {
        Title = "Not all Materials in this Step have been Collected"
    };

    public static ManagedException NotAllStepsCompleted() => new ManagedException
    {
        Title = "Not all Processing Steps have been completed"
    };
}
