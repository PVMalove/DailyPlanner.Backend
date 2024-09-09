using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Persistence.Configurations;
using FluentValidation;

namespace DailyPlanner.Application.Validations.FluentValidator;

public class UpdateReportValidator : AbstractValidator<UpdateReportDto>
{
    public UpdateReportValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(Constants.MAX_LOW_TEXT_LENGTH);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH);
    }
}