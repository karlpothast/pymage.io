using System;

namespace Application.Common.Responses;

public record Jwt
{
    public string Token { get; init; } = null!;
    public DateTime ExpDate { get; init; }
}