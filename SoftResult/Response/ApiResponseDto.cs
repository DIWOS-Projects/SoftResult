using SoftResult.Enums;
using SoftResult.Interfaces;
using System.Text.Json.Serialization;

namespace SoftResult.Response;

/// <summary>
/// DTO for standardizing API responses.
/// </summary>
internal sealed class ApiResponseDto<T>
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("locale")]
    public Locale Locale { get; set; }

    [JsonPropertyName("messages")]
    public IReadOnlyCollection<string> Messages { get; set; } = [];

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Value { get; set; }

    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyCollection<IError>? Errors { get; set; }
}