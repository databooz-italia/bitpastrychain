using System;
using Newtonsoft.Json;

namespace BitPastry.Backend.DTO.HTTPMessages.Response;

public class AuthenticationResponse
{
    [JsonProperty("id")]
    public int ID { get; set; }
    [JsonProperty("Username")]
    public string Username { get; set; } = null!;
    [JsonProperty("Token")]
    public string Token { get; set; } = null!;

    [JsonProperty("FullName")]
    public string? FullName { get; set; }

}


