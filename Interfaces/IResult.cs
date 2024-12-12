using Microsoft.AspNetCore.Mvc;
using SoftResult.Enums;

namespace SoftResult.Interfaces;

/// <summary>
/// Интерфейс результата
/// </summary>
public interface IResult : IActionResult
{
    /// <summary>
    /// Язык
    /// </summary>
    public Locale Locale { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public IEnumerable<string> Messages { get; init; }

    /// <summary>
    /// Ошибки
    /// </summary>
    public IEnumerable<IError>? Errors { get; init; }

    /// <summary>
    /// Статус
    /// </summary>
    public bool IsSuccess => Errors == null || !Errors.Any();
}

/// <summary>
/// Интерфейс результата с возвращаемым значением конкретного типа
/// </summary>
/// <typeparam name="T">  Тип возвращаемого значения </typeparam>
public interface IResult<T> : IResult where T : class
{
    /// <summary>
    /// Возвращаемое значение
    /// </summary>
    public T? Value { get; set; }
}