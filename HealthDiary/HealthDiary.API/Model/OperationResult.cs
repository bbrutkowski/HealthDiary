namespace HealthDiary.API.Model
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }
    }

    public static class OperationResultExtensions
    {
        public static OperationResult Success() => new OperationResult { IsSuccess = true, IsFailure = false };

        public static OperationResult<T> Success<T>(T data) => new OperationResult<T> { IsSuccess = true, IsFailure = false, Data = data };

        public static OperationResult Failure(string error) => new OperationResult { IsFailure = true, IsSuccess = false, ErrorMessage = error };

        public static OperationResult<T> Failure<T>(string error) => new OperationResult<T> { IsFailure = true, IsSuccess = false, ErrorMessage = error };
    }
}
