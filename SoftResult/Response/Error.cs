using SoftResult.Extensions;
using SoftResult.Interfaces;

namespace SoftResult.Response;

/// <summary>
/// Represents a standardized error object.
/// </summary>
public sealed class Error : IError
{
    /// <summary>
    /// Gets the error message
    /// </summary>
    public string Message { get; private init; }

    /// <summary>
    /// Gets the dictionary of error details, containing the element with the error and the reason
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with a message and metadata
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="metadata">The dictionary of error details</param>
    internal Error(string message, IReadOnlyDictionary<string, object>? metadata = null)
    {
        Message = message;
        Metadata = metadata ?? new Dictionary<string, object>().AsReadOnly();
    }

    /// <summary>
    /// Creates a new error with a default message
    /// </summary>
    /// <returns>A new <see cref="IError"/> instance</returns>
    public static IError Create()
    {
        return new Error("Error");
    }

    /// <summary>
    /// Creates a new error with the specified message
    /// </summary>
    /// <param name="message">The error message</param>
    /// <returns>A new <see cref="IError"/> instance</returns>
    public static IError Create(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        return new Error(message);
    }

    /// <summary>
    /// Creates a new error with the specified message and metadata
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="metadata">The dictionary of error details</param>
    /// <returns>A new <see cref="IError"/> instance</returns>
    public static IError Create(string message, IReadOnlyDictionary<string, object> metadata)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentNullException.ThrowIfNull(metadata);
        return new Error(message, metadata);
    }

    /// <summary>
    /// Creates a new error from a dictionary of metadata. The message is constructed from the metadata
    /// </summary>
    /// <param name="metadata">The dictionary of error details</param>
    /// <returns>A new <see cref="IError"/> instance</returns>
    public static IError FromMetadata(IReadOnlyDictionary<string, object> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        var message = metadata.MessagesToString();
        return new Error(message, metadata);
    }

    /// <summary>
    /// Creates a new error from a key-value pair, where the key is the element with the error
    /// and the value is the description of the error reason
    /// </summary>
    /// <param name="key">The element with the error</param>
    /// <param name="value">The description of the error reason</param>
    /// <returns>A new <see cref="IError"/> instance</returns>
    public static IError FromKeyValue(string key, object value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);
        var message = $"{key}: {value}";
        var metadata = new Dictionary<string, object> { { key, value } }.AsReadOnly();
        return new Error(message, metadata);
    }
}