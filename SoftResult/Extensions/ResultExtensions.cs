using SoftResult.Interfaces;
using SoftResult.Response;

namespace SoftResult.Extensions;

/// <summary>
/// Helper class for creating instances of Result without explicitly specifying the generic argument
/// </summary>
public static class ResultExtensions
{
    #region Synchronous methods

    /// <summary>
    /// Creates a successful result (HTTP 200 OK) with the specified type of return value
    /// </summary>
    public static IResult<T> Ok<T>(T value) => Result<T>.Ok(value);

    /// <summary>
    /// Creates a successful result (HTTP 200 OK) for an asynchronous stream
    /// </summary>
    public static IResult<T> Ok<T>(IAsyncEnumerable<T> value) => Result<T>.Ok(value);

    /// <summary>
    /// Creates a successful result (HTTP 200 OK) with a custom message
    /// </summary>
    public static IResult<T> Ok<T>(string message, T value) => Result<T>.Ok(message, value);

    /// <summary>
    /// Returns an error result (HTTP 400 Bad Request) with the specified message
    /// </summary>
    public static IResult<T> BadRequest<T>(string message) => Result<T>.BadRequest(message);

    /// <summary>
    /// Returns a "Bad Request" result with an error object
    /// </summary>
    public static IResult<T> BadRequest<T>(IError error) => Result<T>.BadRequest(error);

    /// <summary>
    /// Returns an error result (HTTP 400 Bad Request) with a collection of errors
    /// </summary>
    public static IResult<T> BadRequest<T>(IEnumerable<IError> errorsList) => Result<T>.BadRequest(errorsList);

    /// <summary>
    /// Returns an error result (HTTP 400 Bad Request) with a message, key, and additional data value
    /// </summary>
    public static IResult<T> BadRequest<T>(string errorMessage, string key, object value) => Result<T>.BadRequest(errorMessage, key, value);

    /// <summary>
    /// Returns a "not found" result (HTTP 404 Not Found) with the specified message
    /// </summary>
    public static IResult<T> NotFound<T>(string message) => Result<T>.NotFound(message);

    /// <summary>
    /// Returns a "not found" result (HTTP 404 Not Found) with the provided error
    /// </summary>
    public static IResult<T> NotFound<T>(IError error) => Result<T>.NotFound(error);

    /// <summary>
    /// Returns a "not found" result (HTTP 404 Not Found) with a collection of errors
    /// </summary>
    public static IResult<T> NotFound<T>(IEnumerable<IError> errorsList) => Result<T>.NotFound(errorsList);

    /// <summary>
    /// Returns an error result (HTTP 404 Not Found) with a message, key, and additional data value
    /// </summary>
    public static IResult<T> NotFound<T>(string errorMessage, string key, object value) => Result<T>.NotFound(errorMessage, key, value);

    /// <summary>
    /// Returns a result with no content (HTTP 204 No Content)
    /// </summary>
    public static IResult<T> NoContent<T>(string message) => Result<T>.NoContent(message);

    #endregion

    #region Asynchronous methods

    /// <summary>
    /// Asynchronously creates a successful result (HTTP 200 OK) with the specified type of return value
    /// </summary>
    public static Task<IResult<T>> OkAsync<T>(T value) => Result<T>.OkAsync(value);

    /// <summary>
    /// Asynchronously creates a successful result (HTTP 200 OK) for an asynchronous stream
    /// </summary>
    public static Task<IResult<T>> OkAsync<T>(IAsyncEnumerable<T> value) => Result<T>.OkAsync(value);

    /// <summary>
    /// Asynchronously creates a successful result (HTTP 200 OK) with a custom message
    /// </summary>
    public static Task<IResult<T>> OkAsync<T>(string message, T value) => Result<T>.OkAsync(message, value);

    /// <summary>
    /// Asynchronously returns an error result (HTTP 400 Bad Request) with the specified message
    /// </summary>
    public static Task<IResult<T>> BadRequestAsync<T>(string message) => Result<T>.BadRequestAsync(message);

    /// <summary>
    /// Asynchronously returns a "Bad Request" result with an error object
    /// </summary>
    public static Task<IResult<T>> BadRequestAsync<T>(IError error) => Result<T>.BadRequestAsync(error);

    /// <summary>
    /// Asynchronously returns an error result (HTTP 400 Bad Request) with a collection of errors
    /// </summary>
    public static Task<IResult<T>> BadRequestAsync<T>(IEnumerable<IError> errorsList) => Result<T>.BadRequestAsync(errorsList);

    /// <summary>
    /// Asynchronously returns an error result (HTTP 400 Bad Request) with a message, key, and additional data value
    /// </summary>
    public static Task<IResult<T>> BadRequestAsync<T>(string errorMessage, string key, object value) => Result<T>.BadRequestAsync(errorMessage, key, value);

    /// <summary>
    /// Asynchronously returns a "not found" result (HTTP 404 Not Found) with the specified message
    /// </summary>
    public static Task<IResult<T>> NotFoundAsync<T>(string message) => Result<T>.NotFoundAsync(message);

    /// <summary>
    /// Asynchronously returns a "not found" result (HTTP 404 Not Found) with the provided error
    /// </summary>
    public static Task<IResult<T>> NotFoundAsync<T>(IError error) => Result<T>.NotFoundAsync(error);

    /// <summary>
    /// Asynchronously returns a "not found" result (HTTP 404 Not Found) with a collection of errors
    /// </summary>
    public static Task<IResult<T>> NotFoundAsync<T>(IEnumerable<IError> errorsList) => Result<T>.NotFoundAsync(errorsList);

    /// <summary>
    /// Asynchronously returns an error result (HTTP 404 Not Found) with a message, key, and additional data value
    /// </summary>
    public static Task<IResult<T>> NotFoundAsync<T>(string errorMessage, string key, object value) => Result<T>.NotFoundAsync(errorMessage, key, value);

    /// <summary>
    /// Asynchronously returns a result with no content (HTTP 204 No Content)
    /// </summary>
    public static Task<IResult<T>> NoContentAsync<T>(string message) => Result<T>.NoContentAsync(message);

    #endregion
}