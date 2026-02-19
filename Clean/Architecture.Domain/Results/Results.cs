using System.Net;

namespace Architecture.Domain.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Message { get; protected set; }
        public List<string> Errors { get; protected set; }
        public HttpStatusCode StatusCode { get; protected set; }

        protected Result(bool isSuccess, HttpStatusCode statusCode, string message, List<string> errors = null)
        {
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Errors = errors ?? new List<string>();
        }

        public static Result Success() => new Result(true, HttpStatusCode.OK, string.Empty);
        public static Result<T> Success<T>(T value) => new Result<T>(true, value, HttpStatusCode.OK, string.Empty);
        public static Result Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new Result(false, statusCode, message);
        public static Result<T> Fail<T>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new Result<T>(false, default, statusCode, message);
        public static Result NotFound(string message = "Resource not found") => new Result(false, HttpStatusCode.NotFound, message);
        public static Result<T> NotFound<T>(string message = "Resource not found") => new Result<T>(false, default, HttpStatusCode.NotFound, message);
        public static Result BadRequest(string message) => new Result(false, HttpStatusCode.BadRequest, message);
        public static Result<T> BadRequest<T>(string message) => new Result<T>(false, default, HttpStatusCode.BadRequest, message);
        public static Result Unauthorized(string message = "Unauthorized") => new Result(false, HttpStatusCode.Unauthorized, message);
        public static Result<T> Unauthorized<T>(string message = "Unauthorized") => new Result<T>(false, default, HttpStatusCode.Unauthorized, message);
        public static Result Forbidden(string message = "Forbidden") => new Result(false, HttpStatusCode.Forbidden, message);
        public static Result<T> Forbidden<T>(string message = "Forbidden") => new Result<T>(false, default, HttpStatusCode.Forbidden, message);
        public static Result Internal(string message) => new Result(false, HttpStatusCode.InternalServerError, message);
        public static Result<T> Internal<T>(string message) => new Result<T>(false, default, HttpStatusCode.InternalServerError, message);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        internal Result(bool isSuccess, T value, HttpStatusCode statusCode, string message, List<string> errors = null) : base(isSuccess, statusCode, message, errors)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T value) => Success(value);
    }
}