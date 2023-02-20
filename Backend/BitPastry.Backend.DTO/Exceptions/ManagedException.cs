using System;
namespace BitPastry.Backend.DTO.Exceptions;

public class ManagedException : Exception
{
    public string? Title { get; init; }
    public Status StatusCode { get; init; } = Status.BadRequest;
    public string? Detail { get; init; }
    public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    public ManagedException AddExtensions(
        string key,
        object? value
    ) {
        this.Extensions.Add(key, value);
        return this;
    }
}

