using System;
namespace BitPastry.Backend.DTO.Models;

public class AccountProxy
{
    public int ID { get; set; }
    public int? IDOperatore { get; set; }
    public string Username { get; set; } = null!;
    public string? FullName { get; set; }
}

