using DailyPlanner.Domain.Enum;

namespace DailyPlanner.Domain.Result;


public class CollectionResult<T> : BaseResult<IEnumerable<T>>
{
    public int Count { get; }
    
    public CollectionResult(string errorMessage, ErrorCodes errorCode) 
        : base(errorMessage, errorCode)
    { }

    public CollectionResult(IEnumerable<T> data, int count)
        : this(string.Empty, ErrorCodes.None)
    {
        Count = count;
    }
}