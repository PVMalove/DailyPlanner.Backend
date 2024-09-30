﻿using DailyPlanner.Domain.DTO.Report;
using DailyPlanner.Test.Configurations;
using Diary.Tests;
using Xunit;

namespace DailyPlanner.Test;

public class ReportServiceTest
{
    [Fact]
    public async Task GetReport_ShouldBe_NotNull()
    {
        //Arrange
        var reportService = ReportServiceFields.GetService();
        //Act
        var result = await reportService.GetReportByIdAsync(1);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateReport_ShouldBe_NewReport()
    {
        //Arrange
        var user = MockRepositoriesGetter.GetUsers().FirstOrDefault();
        var createReportDto = new CreateReportDto("UnitTestReport3", "UnitTestDescription3", user.Id);
        var reportService = ReportServiceFields.GetService();

        //Act
        var result = await reportService.CreateReportAsync(createReportDto);

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteReport_ShouldBe_Return_TrueSuccess()
    {
        // Arrange
        var report = MockRepositoriesGetter.GetReports().FirstOrDefault();
        var reportService = ReportServiceFields.GetService();

        // Act
        var result = await reportService.DeleteReportAsync(report.Id);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateReport_ShouldBe_Return_NewData_For_Report()
    {
        // Arrange
        var report = MockRepositoriesGetter.GetReports().FirstOrDefault();
        var updateReportDto = new UpdateReportDto(report.Id, "UnitTest New name for report",
            "UnitTest New description for report");
        var reportService = ReportServiceFields.GetService();

        // Act
        var result = await reportService.UpdateReportAsync(updateReportDto);

        // Assert
        Assert.True(result.IsSuccess);
    }
}