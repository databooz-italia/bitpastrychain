using System;
using Newtonsoft.Json;

namespace BitPastry.Backend.DTO.HTTPMessages.Request;

public class AuthenticationRequest
{
    [JsonProperty("Username")]
    public string? Username { get; set; }

    [JsonProperty("Password")]
    public string? Password { get; set; }

    [JsonProperty("IsRememberMe")]
    public bool IsRememberMe { get; set; }
}


