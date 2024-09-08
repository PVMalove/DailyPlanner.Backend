using DailyPlanner.Domain.Result;

namespace DailyPlanner.Domain.Interfaces.Validations;

public interface IBaseValidator<in T>  where T : class
{
    BaseResult ValidateOnNull(T entity); 
}