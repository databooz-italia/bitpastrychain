using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitPastry.Backend.DTO.Exceptions;
public enum Status : int {
    // 4xx Client Error
    BadRequest = 400,
    Forbidden = 403, // Autorizzazione
    NotFound = 404,
}
