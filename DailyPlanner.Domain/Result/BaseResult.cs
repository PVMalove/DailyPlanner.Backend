using DailyPlanner.Domain.Enum;

namespace DailyPlanner.Domain.Result;


public class BaseResult(string errorMessage, ErrorCodes errorCode)
{
    /// <summary>
    /// Возвращает true, если операция была успешной, false - если произошла ошибка.
    /// </summary>
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public string ErrorMessage { get; init; } = errorMessage ?? string.Empty;
    public ErrorCodes ErrorCode { get; init; } = errorCode;
    
    public BaseResult() : this(string.Empty, ErrorCodes.None) { }
}

public class BaseResult<T> : BaseResult
{
    public T Data { get; init; }
    
    public BaseResult(string errorMessage, ErrorCodes errorCode):
        base(errorMessage, errorCode) 
    { }
    
    public BaseResult(T data)
        : this(string.Empty, ErrorCodes.None)
    {
        Data = data;
    }
}