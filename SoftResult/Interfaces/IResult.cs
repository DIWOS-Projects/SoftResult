using Microsoft.AspNetCore.Mvc;
using SoftResult.Enums;

namespace SoftResult.Interfaces;

/// <summary>
/// Result interface
/// </summary>
public interface IResult : IActionResult
{
    /// <summary>
    /// Language
    /// </summary>
    public Locale Locale { get; set; }

    /// <summary>
    /// Messages
    /// </summary>
    public IReadOnlyCollection<string> Messages { get; init; }

    /// <summary>
    /// Errors
    /// </summary>
    public IReadOnlyCollection<IError> Errors { get; init; }

    /// <summary>
    /// Status
    /// </summary>
    public bool IsSuccess
    {
        get
        {
            return Errors.Count == 0;
        }
    }
}

/// <summary>
/// Result interface with a specific return value type
/// </summary>
/// <typeparam name="T">The type of the return value</typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    /// Return value
    /// </summary>
    public T? Value { get; set; }
}