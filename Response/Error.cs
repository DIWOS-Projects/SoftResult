using SoftResult.Extensions;
using SoftResult.Interfaces;

namespace SoftResult.Response;

/// <summary>
/// Ошибка
/// </summary>
public sealed class Error : IError
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Справочник деталей ошибки
    /// элемент с ошибкой, причина ошибки
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Базовый случай ошибки
    /// </summary>
    public Error()
    {
        Message = "Error";
    }

    /// <summary>
    /// Ошибка с заданным текстом
    /// </summary>
    /// <param name="message">  Сообщение об ошибке  </param>
    public Error(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Ошибка с заданным текстом и
    /// </summary>
    /// <param name="message">  Сообщение об ошибке  </param>
    /// <param name="metadata">  Детали ошибки  </param>
    public Error(string message, IReadOnlyDictionary<string, object> metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <summary>
    /// Ошибка со справочником деталей ошибки
    /// </summary>
    /// <param name="metadata">  Детали ошибки  </param>
    public Error(IReadOnlyDictionary<string, object> metadata)
    {
        Message = metadata.MessagesToString();
        Metadata = metadata;
    }

    /// <summary>
    /// Ошибка с деталью ошибки по ключ, значение
    /// ключ - элемент с ошибкой
    /// значение - описание причины ошибки в эоементе
    /// </summary>
    /// <param name="key">  Элемент с ошибкой  </param>
    /// <param name="value">  Описание причины ошибки  </param>
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