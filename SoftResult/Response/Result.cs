using Microsoft.AspNetCore.Mvc;
using SoftResult.Enums;
using SoftResult.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable MemberCanBePrivate.Global

namespace SoftResult.Response;

/// <summary>
/// Result
/// </summary>
/// <typeparam name="T">Type of the return value</typeparam>
public sealed class Result<T> : IResult<T>
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    /// Type of the return value
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// Localization for the result message
    /// </summary>
    public Locale Locale { get; set; } = Locale.Rus;

    /// <summary>
    /// Response code
    /// </summary>
    private int StatusCode { get; set; }

    /// <summary>
    /// List of messages
    /// </summary>
    public IReadOnlyCollection<string> Messages { get; set; } = [];

    /// <summary>
    /// List of errors
    /// </summary>
    public IReadOnlyCollection<Error> Errors { get; set; } = [];

    /// <summary>
    /// Status
    /// </summary>
    public bool IsSuccess { get; set; }

    // Синхронные методы

    /// <summary>
    /// Creates a successful result (HTTP 200 OK) with the specified type of return value
    /// </summary>
    /// <param name="value">Value to be returned in the result</param>
    /// <returns>Result object with a 200 OK code</returns>
    public static IResult<T> Ok(T value) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status200OK,
        Value = value,
        Messages = new List<string> { "Ok" }.AsReadOnly()
    };

    /// <summary>
    /// Creates a successful result (HTTP 200 OK) for an asynchronous stream
    /// </summary>
    /// <param name="value">The asynchronous stream to be returned</param>
    /// <returns>A result object with a 200 OK status code</returns>
    public static IResult<T> Ok(IAsyncEnumerable<T> value) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status200OK,
        Value = (T)value,
        Messages = ["Stream successfully retrieved"]
    };

    /// <summary>
    /// Creates a successful result (HTTP 200 OK) with a custom message and specified value type
    /// </summary>
    /// <param name="message">Message indicating successful execution</param>
    /// <param name="value">Value to be returned</param>
    /// <returns>Result object with a 200 OK code</returns>
    public static IResult<T> Ok(string message, T value) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status200OK,
        Value = value,
        Messages = [message]
    };

    /// <summary>
    /// Returns an error result (HTTP 400 Bad Request) with the specified message
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Result with a 400 Bad Request code</returns>
    public static IResult<T> BadRequest(string message) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Messages = [message],
        Errors = [new Error(message)]
    };

    /// <summary>
    /// Returns a "Bad Request" result with an error object
    /// </summary>
    /// <param name="error">Error object</param>
    /// <returns>Result with a 400 Bad Request code</returns>
    public static IResult<T> BadRequest(IError error) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Messages = [error.Message],
        Errors = [error is Error e ? e : new Error(error.Message, error.Metadata)]
    };

    /// <summary>
    /// Returns an error result (HTTP 400 Bad Request) with a collection of errors
    /// </summary>
    /// <param name="errorsList">List of errors</param>
    /// <returns>Result with a 400 Bad Request code</returns>
    public static IResult<T> BadRequest(IEnumerable<IError> errorsList)
    {
        if (errorsList == null)
            throw new ArgumentException("The list of errors cannot be null", nameof(errorsList));

        var dataList = errorsList.ToList();
        if (dataList.Count == 0)
            throw new ArgumentException("The list of errors cannot be empty", nameof(errorsList));

        return new Result<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Messages = dataList
                .Select(e => e.Message)
                .ToList(),
            Errors = dataList
                .Select(e => e is Error er ? er : new Error(e.Message, e.Metadata))
                .ToList()
        };
    }

    /// <summary>
    /// Returns an error result (HTTP 400 Bad Request) with a message, key, and additional data value
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <param name="key">Key for error metadata</param>
    /// <param name="value">Value associated with the key</param>
    /// <returns>Result with a 400 Bad Request code</returns>
    public static IResult<T> BadRequest(string errorMessage, string key, object value) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Messages = [errorMessage],
        Errors =
        [
            new Error(errorMessage, new Dictionary<string, object> { { key, value } })
        ]
    };

    /// <summary>
    /// Returns a "not found" result (HTTP 404 Not Found) with the specified message
    /// </summary>
    /// <param name="message">Message describing the reason</param>
    /// <returns>Result with a 404 Not Found code</returns>
    public static IResult<T> NotFound(string message) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status404NotFound,
        Messages = [message],
        Errors = [new Error(message)]
    };

    /// <summary>
    /// Returns a "not found" result (HTTP 404 Not Found) with the provided error
    /// </summary>
    /// <param name="error">Error object</param>
    /// <returns>Result with a 404 Not Found code</returns>
    public static IResult<T> NotFound(IError error) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status404NotFound,
        Messages = [error.Message],
        Errors = [error is Error e ? e : new Error(error.Message, error.Metadata)]
    };

    /// <summary>
    /// Returns a "not found" result (HTTP 404 Not Found) with a collection of errors
    /// </summary>
    /// <param name="errorsList">List of errors</param>
    /// <returns>Result with a 404 Not Found code</returns>
    public static IResult<T> NotFound(IEnumerable<IError> errorsList)
    {
        if (errorsList == null)
            throw new ArgumentException("The list of errors cannot be null", nameof(errorsList));

        var dataList = errorsList.ToList();
        if (dataList.Count == 0)
            throw new ArgumentException("The list of errors cannot be empty", nameof(errorsList));

        return new Result<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status404NotFound,
            Messages = dataList
                .Select(e => e.Message)
                .ToList(),
            Errors = dataList
                .Select(e => e is Error er ? er : new Error(e.Message, e.Metadata))
                .ToList()
        };
    }

    /// <summary>
    /// Returns an error result (HTTP 404 Not Found) with an error message, key, and additional data
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <param name="key">Key for metadata</param>
    /// <param name="value">Value associated with the key</param>
    /// <returns>Result with a 404 Not Found code</returns>
    public static IResult<T> NotFound(string errorMessage, string key, object value) => new Result<T>
    {
        IsSuccess = false,
        StatusCode = StatusCodes.Status404NotFound,
        Messages = [errorMessage],
        Errors =
        [
            new Error(errorMessage, new Dictionary<string, object> { { key, value } })
        ]
    };

    /// <summary>
    /// Returns a result with no content (HTTP 204 No Content)
    /// </summary>
    /// <param name="message">Message describing the result</param>
    /// <returns>Result with a 204 No Content code</returns>
    public static IResult<T> NoContent(string message) => new Result<T>
    {
        IsSuccess = true,
        StatusCode = StatusCodes.Status204NoContent,
        Messages = [message],
        Value = default
    };

    // Асинхронные методы

    /// <summary>
    /// Asynchronously creates a successful result (HTTP 200 OK) with the specified type of return value
    /// </summary>
    /// <param name="value">Value to be returned in the result</param>
    /// <returns>Task containing a result object with a 200 OK code</returns>
    public static Task<IResult<T>> OkAsync(T value)
        => Task.FromResult(Ok(value));

    /// <summary>
    /// Asynchronously creates a successful result (HTTP 200 OK) for an asynchronous stream
    /// </summary>
    /// <param name="value">The asynchronous stream to be returned</param>
    /// <returns>A task containing a result object with a 200 OK status code</returns>
    public static Task<IResult<T>> OkAsync(IAsyncEnumerable<T> value)
        => Task.FromResult(Ok(value));

    /// <summary>
    /// Asynchronously creates a successful result (HTTP 200 OK) with a custom message and specified value type
    /// </summary>
    /// <param name="message">Message indicating successful execution</param>
    /// <param name="value">Value to be returned</param>
    /// <returns>Task containing a result object with a 200 OK code</returns>
    public static Task<IResult<T>> OkAsync(string message, T value)
        => Task.FromResult(Ok(message, value));

    /// <summary>
    /// Asynchronously returns an error result (HTTP 400 Bad Request) with the specified message
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Task containing a result with a 400 Bad Request code</returns>
    public static Task<IResult<T>> BadRequestAsync(string message)
        => Task.FromResult(BadRequest(message));

    /// <summary>
    /// Asynchronously returns a "Bad Request" result with an error object
    /// </summary>
    /// <param name="error">Error object</param>
    /// <returns>Task containing a result with a 400 Bad Request code</returns>
    public static Task<IResult<T>> BadRequestAsync(IError error)
        => Task.FromResult(BadRequest(error));

    /// <summary>
    /// Asynchronously returns an error result (HTTP 400 Bad Request) with a collection of errors
    /// </summary>
    /// <param name="errorsList">List of errors</param>
    /// <returns>Task containing a result with a 400 Bad Request code</returns>
    public static Task<IResult<T>> BadRequestAsync(IEnumerable<IError> errorsList)
        => Task.FromResult(BadRequest(errorsList));

    /// <summary>
    /// Asynchronously returns an error result (HTTP 400 Bad Request) with a message, key, and additional data value
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <param name="key">Key for error metadata</param>
    /// <param name="value">Value associated with the key</param>
    /// <returns>Task containing a result with a 400 Bad Request code</returns>
    public static Task<IResult<T>> BadRequestAsync(string errorMessage, string key, object value)
        => Task.FromResult(BadRequest(errorMessage, key, value));

    /// <summary>
    /// Asynchronously returns a "not found" result (HTTP 404 Not Found) with the specified message
    /// </summary>
    /// <param name="message">Message describing the reason</param>
    /// <returns>Task containing a result with a 404 Not Found code</returns>
    public static Task<IResult<T>> NotFoundAsync(string message)
        => Task.FromResult(NotFound(message));

    /// <summary>
    /// Asynchronously returns a "not found" result (HTTP 404 Not Found) with the provided error
    /// </summary>
    /// <param name="error">Error object</param>
    /// <returns>Task containing a result with a 404 Not Found code</returns>
    public static Task<IResult<T>> NotFoundAsync(IError error)
        => Task.FromResult(NotFound(error));

    /// <summary>
    /// Asynchronously returns a "not found" result (HTTP 404 Not Found) with a collection of errors
    /// </summary>
    /// <param name="errorsList">List of errors</param>
    /// <returns>Task containing a result with a 404 Not Found code</returns>
    public static Task<IResult<T>> NotFoundAsync(IEnumerable<IError> errorsList)
        => Task.FromResult(NotFound(errorsList));

    /// <summary>
    /// Asynchronously returns an error result (HTTP 404 Not Found) with an error message, key, and additional data
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <param name="key">Key for metadata</param>
    /// <param name="value">Value associated with the key</param>
    /// <returns>Task containing a result with a 404 Not Found code</returns>
    public static Task<IResult<T>> NotFoundAsync(string errorMessage, string key, object value)
        => Task.FromResult(NotFound(errorMessage, key, value));

    /// <summary>
    /// Asynchronously returns a result with no content (HTTP 204 No Content)
    /// </summary>
    /// <param name="message">Message describing the result</param>
    /// <returns>Task containing a result with a 204 No Content code</returns>
    public static Task<IResult<T>> NoContentAsync(string message)
        => Task.FromResult(NoContent(message));

    /// <summary>
    /// Asynchronously executes the result and returns it in JSON format with the specified status code
    /// </summary>
    /// <param name="context">Action context in which the result is executed</param>
    /// <returns>Asynchronous task</returns>
    public async Task ExecuteResultAsync(ActionContext context)
    {
        try
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            context.HttpContext.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(new
            {
                IsSuccess,
                Locale,
                Messages,
                StatusCode,
                Value,
                Errors
            }, _jsonSerializerOptions);

            await context.HttpContext.Response.WriteAsync(json);
        }
        catch (JsonException ex)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Serialization error: " + ex.Message }));
        }
    }
}