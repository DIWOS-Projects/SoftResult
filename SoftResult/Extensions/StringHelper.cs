using SoftResult.Interfaces;

namespace SoftResult.Extensions;

/// <summary>
/// Extension class for processing result messages
/// </summary>
internal static class StringHelper
{
    /// <summary>
    /// Converts messages from a Result object into a single string
    /// </summary>
    /// <param name="result"> The Result object containing a collection of messages </param>
    /// <returns> A string containing all messages separated by a new line </returns>
    public static string MessagesToString<T>(this IResult<T> result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result), "The result object cannot be null");

        if (result.Messages.Count == 0)
            return string.Empty;

        return result.Messages.Count == 0
            ? string.Empty
            : string.Join(Environment.NewLine, result.Messages);
    }

    /// <summary>
    /// Converts a dictionary of metadata into a string, combining keys and values with a new line separator
    /// </summary>
    /// <param name="value"> Metadata dictionary. </param>
    /// <returns> A string containing keys and values of metadata separated by a new line. </returns>
    /// <exception cref="ArgumentException"> Thrown if the metadata dictionary is empty. </exception>
    public static string MessagesToString(this IReadOnlyDictionary<string, object> value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "The message collection cannot be null when combining all messages into a single string");

        return value.Count == 0
            ? throw new ArgumentException("Metadata cannot be empty when attempting to retrieve a metadata string", nameof(value))
            : string.Join(Environment.NewLine, value.Select(x => x.Key + ": " + x.Value));
    }
}