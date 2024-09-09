using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Persistence.Configurations;
using FluentValidation;

namespace DailyPlanner.Application.Validations.FluentValidator;

public class CreateReportValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(Constants.MAX_LOW_TEXT_LENGTH);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH);
    }
}