namespace soft_result.Extensions;

/// <summary>
/// Класс расширений для обработки сообщений и метаданных результатов
/// </summary>
internal static class ResultExtensions
{
    /// <summary>
    /// Переобразует коллекцию сообщений в строку
    /// </summary>
    /// <param name="value"> Коллекция строковых сообщений. </param>
    /// <returns> Строка, содержащая все сообщения, разделенные новой строкой. </returns>
    /// <exception cref="ArgumentException"> Выбрасывается если коллекция сообщений пуста </exception>
    public static string MessagesToString(this IEnumerable<string> value)
    {
        var enumerable = value.ToList();
        if (enumerable.Count == 0)
            throw new ArgumentException("Список ошибок не может быть пустым при попытке получить строку ошибок", nameof(value));

        return string.Join(Environment.NewLine, enumerable);
    }

    /// <summary>
    /// Преобразует словарь метаданных в строку, объединяя ключи и значения через символ новой строки.
    /// </summary>
    /// <param name="value"> Словарь метаданных. </param>
    /// <returns> Строка, содержащая ключи и значения метаданных, разделенные новой строкой. </returns>
    /// <exception cref="ArgumentException"> Выбрасывается, если словарь метаданных пуст. </exception>
    public static string MessagesToString(this IReadOnlyDictionary<string, object> value)
    {
        if (value.Count == 0)
            throw new ArgumentException("Метаданные не могут быть пустыми при попытке получить строку метаданных", nameof(value));

        return string.Join(Environment.NewLine, value.Select(x => x.Key + ": " + x.Value));
    }
}