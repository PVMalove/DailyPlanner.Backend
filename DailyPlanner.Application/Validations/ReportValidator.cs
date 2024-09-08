using DailyPlanner.Application.Resources;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Enum;
using DailyPlanner.Domain.Interfaces.Validations;
using DailyPlanner.Domain.Result;

namespace DailyPlanner.Application.Validations;

public class ReportValidator : IReportValidator
{
    public BaseResult ValidateOnNull(Report? entity)
    {
        if (entity is null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = ErrorCodes.ReportNotFound
            };
        }

        return new BaseResult();
    }

    public BaseResult ValidateCreating(Report? entity, User? user)
    {
        if (entity is not null)
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.ReportAlreadyExists,
                ErrorCode = ErrorCodes.ReportAlreadyExists
            };
        }

        if (user is null)
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = ErrorCodes.UserNotFound
            };
        }
        return new BaseResult();
    }
}