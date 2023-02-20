using System;
namespace BitPastry.Backend.DTO.Configurations;

public class JWTConfiguration
{
    public string Secret { get; set; }
    public int TokenValidityMinutes { get; set; }
}

