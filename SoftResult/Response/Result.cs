using Microsoft.AspNetCore.Mvc;
using SoftResult.Enums;
using SoftResult.Interfaces;
using System.Text.Json;

namespace SoftResult.Response;

/// <summary>
/// Результат
/// </summary>
/// <typeparam name="T">  Тип возвращаемого значения </typeparam>
public sealed class Result<T> : IResult<T>
{
    /// <summary>
    ///Тип возвращаемого значения
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// Локализация для сообщения результата
    /// </summary>
    public Locale Locale { get; set; } = Locale.Rus;

    /// <summary>
    /// Код ответа
    /// </summary>
    private int StatusCode { get; init; }

    /// <summary>
    /// Список сообщений
    /// </summary>
    public required IEnumerable<string> Messages { get; init; }

    /// <summary>
    /// Список ошибок
    /// </summary>
    public IEnumerable<IError>? Errors { get; init; }

    /// <summary>
    /// Статус
    /// </summary>
    public bool IsSuccess { get; private init; }

    /// <summary>
    /// Создает успешный результат (HTTP 200 OK) с указанным типом возвращаемого сообщения.
    /// </summary>
    /// <param name="value"> Значение, которое будет возвращено в результате. </param>
    /// <returns> Объект результата с кодом 200 OK. </returns>
    public static IResult<T> Ok(T value) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status200OK,
        Value = value,
        Messages = ["Ok"]
    };

    /// <summary>
    /// Создает успешный результат (HTTP 200 OK) с пользовательским сообщением и указанным типом значения.
    /// </summary>
    /// <param name="message"> Сообщение об успешном выполнении. </param>
    /// <param name="value">  </param>
    /// <returns> Объект результата с кодом 200 OK. </returns>
    public static IResult<T> Ok(string message, T value) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status200OK,
        Value = value,
        Messages = [message]
    };

    /// <summary>
    /// Возвращает результат ошибки (HTTP 400 Bad Request) с заданным сообщением.
    /// </summary>
    /// <param name="message"> Сообщение об ошибке. </param>
    /// <returns> Результат с кодом 400 Bad Request. </returns>
    public static IResult<T> BadRequest(string message) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Messages = [message]
    };

    /// <summary>
    /// Возвращает результат "Bad Request" с объектом ошибки.
    /// </summary>
    /// <param name="error"> Объект ошибки. </param>
    /// <returns> Результат с кодом 400 Bad Request. </returns>
    public static IResult<T> BadRequest(IError error) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Messages = [error.Message],
        Errors = [error]
    };

    /// <summary>
    /// Возвращает результат ошибки (HTTP 400 Bad Request) с коллекцией ошибок.
    /// </summary>
    /// <param name="errorsList"> Список ошибок. </param>
    /// <returns> Результат с кодом 400 Bad Request. </returns>
    public static IResult<T> BadRequest(IEnumerable<IError> errorsList)
    {
        var errors = errorsList.ToList();
        return new Result<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Messages = [errors.First().Message],
            Errors = errors
        };
    }

    /// <summary>
    /// Возвращает результат ошибки (HTTP 400 Bad Request) с сообщением, ключом и значением дополнительных данных.
    /// </summary>
    /// <param name="errorMessage"> Сообщение об ошибке. </param>
    /// <param name="key"> Ключ для метаданных ошибки. </param>
    /// <param name="value"> Значение, связанное с ключом. </param>
    /// <returns> Результат с кодом 400 Bad Request. </returns>
    public static IResult<T> BadRequest(string errorMessage, string key, object value) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Messages = [errorMessage],
        Errors =
        [
            new Error
            {
                Message = errorMessage,
                Metadata = new Dictionary<string, object>
                {
                    { key, value }
                }
            }
        ]
    };

    /// <summary>
    /// Возвращает результат "не найдено" (HTTP 404 Not Found) с заданным сообщением.
    /// </summary>
    /// <param name="message"> Сообщение с описанием причины. </param>
    /// <returns> Результат с кодом 404 Not Found. </returns>
    public static IResult<T> NotFound(string message) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status404NotFound,
        Messages = [message]
    };

    /// <summary>
    /// Возвращает результат "не найдено" (HTTP 404 Not Found) с переданной ошибкой.
    /// </summary>
    /// <param name="error"> Объект ошибки. </param>
    /// <returns> Результат с кодом 404 Not Found. </returns>
    public static IResult<T> NotFound(IError error) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status404NotFound,
        Messages = [error.Message],
        Errors = [error]
    };

    /// <summary>
    /// Возвращает результат "не найдено" (HTTP 404 Not Found) с коллекцией ошибок.
    /// </summary>
    /// <param name="errorsList"> Список ошибок. </param>
    /// <returns> Результат с кодом 404 Not Found. </returns>
    public static IResult<T> NotFound(IEnumerable<IError> errorsList)
    {
        var errors = errorsList.ToList();
        return new Result<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status404NotFound,
            Messages = [errors.First().Message],
            Errors = errors
        };
    }

    /// <summary>
    /// Возвращает результат ошибки (HTTP 400 Bad Request) с сообщением об ошибке, ключом и дополнительными данными.
    /// </summary>
    /// <param name="errorMessage">Сообщение об ошибке. </param>
    /// <param name="key">Ключ для метаданных. </param>
    /// <param name="value">Значение, связанное с ключом. </param>
    /// <returns> Результат с кодом 400 Bad Request. </returns>
    public static IResult<T> NotFound(string errorMessage, string key, object value) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status404NotFound,
        Messages = [errorMessage],
        Errors =
        [
            new Error
            {
                Message = errorMessage,
                Metadata = new Dictionary<string, object>
                {
                    { key, value }
                }
            }
        ]
    };
    
    /// <summary>
    /// Возвращает результат без содержимого (HTTP 204 No Content).
    /// </summary>
    /// <returns>Результат с кодом 204 No Content.</returns>
    public static IResult<T> NoContent(string message) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status204NoContent,
        Messages = [message],
        Value = default
    };

    /// <summary>
    /// Асинхронно выполняет результат и возвращает его в формате JSON с указанным кодом состояния.
    /// </summary>
    /// <param name="context">Контекст действия, в котором выполняется результат. </param>
    /// <returns> Асинхронная задача. </returns>
    public async Task ExecuteResultAsync(ActionContext context)
    {
        if (Messages is null)
            throw new ArgumentException("Сообщение не может быть пустым", nameof(Messages));

        context.HttpContext.Response.StatusCode = StatusCode;

        if (Value != null)
        {
            context.HttpContext.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new
            {
                IsSuccess,
                Locale,
                Messages,
                Value
            });

            await context.HttpContext.Response.WriteAsync(json);
        }
        else
        {
            context.HttpContext.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new
            {
                IsSuccess,
                Locale,
                Messages,
                Errors
            });

            await context.HttpContext.Response.WriteAsync(json);
        }
    }
}