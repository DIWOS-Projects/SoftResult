using SoftResult.Extensions;
using SoftResult.Interfaces;

namespace SoftResult.Response;

/// <summary>
/// Error
/// </summary>
public sealed class Error : IError
{
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Dictionary of error details
    /// element with the error, reason for the error
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>().AsReadOnly();

    /// <summary>
    /// Error with specified text
    /// </summary>
    /// <param name="message">Error message</param>
    public Error(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Error with specified text and metadata
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="metadata">Error details</param>
    public Error(string message, IReadOnlyDictionary<string, object> metadata)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }

    /// <summary>
    /// Error with a dictionary of error details
    /// </summary>
    /// <param name="metadata">Error details</param>
    public Error(IReadOnlyDictionary<string, object> metadata)
    {
        if (metadata == null)
            throw new ArgumentNullException(nameof(metadata), "Metadata cannot be null");

        Message = metadata.MessagesToString();
        Metadata = metadata;
    }

    /// <summary>
    /// Error with an error detail by key-value pair
    /// key - element with the error
    /// value - description of the error reason in the element
    /// </summary>
    /// <param name="key">Element with the error</param>
    /// <param name="value">Description of the error reason</param>
    public Error(string key, object value)
    {
        Message = $"{key}: {value}";
        Metadata = new Dictionary<string, object>
        {
            {
                key, value
            }
        };
    }
}