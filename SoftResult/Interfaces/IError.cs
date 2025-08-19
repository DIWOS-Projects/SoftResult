namespace SoftResult.Interfaces;

/// <summary>
/// Error interface
/// </summary>
public interface IError
{
    /// <summary>
    /// Error message
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Dictionary of error details
    /// element with the error, reason for the error
    /// </summary>
    IReadOnlyDictionary<string, object> Metadata { get; set; }
}