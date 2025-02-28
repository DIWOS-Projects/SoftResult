namespace SoftResult.Extensions;

/// <summary>
/// Extension class for processing result messages and metadata
/// </summary>
internal static class ResultExtensions
{
    /// <summary>
    /// Converts a collection of messages into a single string
    /// </summary>
    /// <param name="value"> Collection of string messages. </param>
    /// <returns> A string containing all messages separated by a new line. </returns>
    /// <exception cref="ArgumentException"> Thrown if the message collection is empty </exception>
    public static string MessagesToString(this IEnumerable<string> value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "The message collection cannot be null");

        var enumerable = value.ToList();
        if (enumerable.Count == 0)
            throw new ArgumentException("The list of errors cannot be empty when attempting to retrieve an error string", nameof(value));

        return string.Join(Environment.NewLine, enumerable);
    }

    /// <summary>
    /// Converts a dictionary of metadata into a string, combining keys and values with a new line separator.
    /// </summary>
    /// <param name="value"> Metadata dictionary. </param>
    /// <returns> A string containing keys and values of metadata separated by a new line. </returns>
    /// <exception cref="ArgumentException"> Thrown if the metadata dictionary is empty. </exception>
    public static string MessagesToString(this IReadOnlyDictionary<string, object> value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "The message collection cannot be null when combining all messages into a single string");

        if (value.Count == 0)
            throw new ArgumentException("Metadata cannot be empty when attempting to retrieve a metadata string", nameof(value));

        return string.Join(Environment.NewLine, value.Select(x => x.Key + ": " + x.Value));
    }
}