using System;
using System.Collections.Generic;
using System.Text;

namespace BitPastry.Backend.Core.Interfaces;

public interface IAuthenticationProvider
{
    int GetLoggedAccountId();
    string GetMachineID();
}
