namespace soft_result.Interfaces;

/// <summary>
/// Интерфейс ошибки
/// </summary>
public interface IError
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Справочник деталей ошибки
    /// элемент с ошибкой, причина ошибки
    /// </summary>
    IReadOnlyDictionary<string, object> Metadata { get; set; }
}