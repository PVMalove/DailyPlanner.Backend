using AutoMapper;
using DailyPlanner.Application.Services;
using DailyPlanner.Application.Validations;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Repository;
using DailyPlanner.Domain.Settings;
using DailyPlanner.Producer;
using DailyPlanner.Test.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using MapperConfiguration = DailyPlanner.Test.Configurations.MapperConfiguration;

namespace Diary.Tests;

public static class ReportServiceFields
{
    private static readonly Mock<IBaseRepository<Report>> MockReportRepository =
        MockRepositoriesGetter.GetMockReportRepository();

    private static readonly Mock<IBaseRepository<User>> MockUserRepository =
        MockRepositoriesGetter.GetMockUserRepository();

    private static readonly Mock<IDistributedCache> MockDistributedCache = new();
    private static readonly IMapper Mapper = MapperConfiguration.GetMapperConfiguration();
    private static readonly User User = MockRepositoriesGetter.GetUsers().FirstOrDefault()!;
    private static readonly Mock<ReportValidator> MockValidator = new();
    private static readonly Mock<IOptions<RabbitMqSettings>> MockRabbitMqOptions = new();

    private static readonly Mock<Producer> MockMessageProducer = new();

    private static readonly ReportService ReportService = new ReportService(
        MockReportRepository.Object,
        MockUserRepository.Object,
        MockValidator.Object,
        MockMessageProducer.Object,
        MockRabbitMqOptions.Object,
        Mapper, 
        null);

    static ReportServiceFields()
    {
        var basePath = AppContext.BaseDirectory;
        var projectPath = Directory.GetParent(basePath)?.Parent?.Parent?.Parent?.Parent?.FullName;
        if (projectPath != null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("DailyPlanner.Api/appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var rabbitMqSettings = new RabbitMqSettings();
            configuration.GetSection("RabbitMqSettings").Bind(rabbitMqSettings);
            var options = Options.Create(rabbitMqSettings);

            MockRabbitMqOptions.Setup(o => o.Value).Returns(options.Value);
        }
    }

    public static ReportService GetService() => ReportService;
}