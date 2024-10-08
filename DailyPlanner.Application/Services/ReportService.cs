﻿using AutoMapper;
using DailyPlanner.Application.Resources;
using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Enum;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Domain.Interfaces.Services;
using DailyPlanner.Domain.Interfaces.Validations;
using DailyPlanner.Domain.Result;
using DailyPlanner.Domain.Settings;
using DailyPlanner.Producer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace DailyPlanner.Application.Services;

public class ReportService : IReportService
{
    private readonly IBaseRepository<Report> reportRepository;
    private readonly IBaseRepository<User> userRepository;
    private readonly IReportValidator reportValidator;
    private readonly IMessageProducer massageProducer;
    private readonly IOptions<RabbitMqSettings> rabbitMqOptions;
    private readonly IMapper mapper;
    private readonly ILogger? logger;

    public ReportService(IBaseRepository<Report> reportRepository, IBaseRepository<User> userRepository,
        IReportValidator reportValidator, IMessageProducer massageProducer, IOptions<RabbitMqSettings> rabbitMqOptions, IMapper mapper, ILogger? logger)
    {
        this.reportRepository = reportRepository;
        this.userRepository = userRepository;
        this.reportValidator = reportValidator;
        this.massageProducer = massageProducer;
        this.rabbitMqOptions = rabbitMqOptions;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId)
    {
        var reports = await reportRepository.GetAll().AsNoTracking()
            .Where(report => report.UserId == userId)
            .Select(report =>
                new ReportDto(report.Id, report.Name, report.Description, report.CreatedAt.ToLongDateString()))
            .ToArrayAsync();

        if (reports.Length > 0)
        {
            return new CollectionResult<ReportDto>(reports, reports.Length);
        }

        logger.Warning(ErrorMessage.ReportsNotFound, reports.Length);
        return new CollectionResult<ReportDto>(
            errorMessage: ErrorMessage.ReportsNotFound,
            errorCode: ErrorCodes.ReportsNotFound);
    }

    public async Task<BaseResult<ReportDto>> GetReportByIdAsync(long id)
    {
        var reports = await reportRepository.GetAll().AsNoTracking()
            .Where(report => report.Id == id)
            .ToListAsync();

        var report = reports.Select(report =>
                new ReportDto(report.Id, report.Name, report.Description, report.CreatedAt.ToLongDateString()))
            .FirstOrDefault(dto => dto.Id == id);

        if (report is not null)
        {
            return new BaseResult<ReportDto>(data: report);
        }

        logger.Warning($"{ErrorMessage.ReportNotFound} - Id: {id}", id);
        return new BaseResult<ReportDto>(
            errorMessage: ErrorMessage.ReportNotFound,
            errorCode: ErrorCodes.ReportNotFound);
    }

    /// <inheritdoc />
    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto reportDto)
    {
        var user = await userRepository.GetAll().AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == reportDto.UserId);

        var report = await reportRepository.GetAll()
            .FirstOrDefaultAsync(report => report.Name == reportDto.Name);

        var result = reportValidator.ValidateCreating(report, user);

        if (!result.IsSuccess)
        {
            return new BaseResult<ReportDto>(result.ErrorMessage, result.ErrorCode);
        }

        report = new Report
        {
            Name = reportDto.Name,
            Description = reportDto.Description,
            UserId = user!.Id
        };

        await reportRepository.CreateAsync(report);
        await reportRepository.SaveChangesAsync();

        massageProducer.SendMessage(report, rabbitMqOptions.Value.RoutingKey, rabbitMqOptions.Value.ExchangeName);

        return new BaseResult<ReportDto>(mapper.Map<ReportDto>(report));
    }

    /// <inheritdoc />
    public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto reportDto)
    {
        var report = await reportRepository.GetAll()
            .FirstOrDefaultAsync(report => report.Id == reportDto.Id);

        var result = reportValidator.ValidateOnNull(report);

        if (!result.IsSuccess)
        {
            logger.Warning($"{ErrorMessage.ReportNotFound} - Id: {reportDto.Id}", reportDto.Id);
            return new BaseResult<ReportDto>(result.ErrorMessage, result.ErrorCode);
        }

        report!.Name = reportDto.Name;
        report.Description = reportDto.Description;

        var updateReport = reportRepository.Update(report);
        await reportRepository.SaveChangesAsync();

        return new BaseResult<ReportDto>(data: mapper.Map<ReportDto>(updateReport));
    }

    /// <inheritdoc />
    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
    {
        var report = await reportRepository.GetAll()
            .FirstOrDefaultAsync(report => report.Id == id);

        var result = reportValidator.ValidateOnNull(report);

        if (!result.IsSuccess)
        {
            logger.Warning($"{ErrorMessage.ReportNotFound} - Id: {id}", id);
            return new BaseResult<ReportDto>(result.ErrorMessage, result.ErrorCode);
        }

        reportRepository.Remove(report!);
        await reportRepository.SaveChangesAsync();

        return new BaseResult<ReportDto>(data: mapper.Map<ReportDto>(report));
    }
}