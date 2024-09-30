using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Interfaces.Repository;
using MockQueryable.Moq;
using Moq;

namespace DailyPlanner.Test.Configurations;

public static class MockRepositoriesGetter
{
    public static Mock<IBaseRepository<Report>> GetMockReportRepository()
    {
        var mock = new Mock<IBaseRepository<Report>>();
        var reports = GetReports().BuildMockDbSet();
        mock.Setup(x => x.GetAll()).Returns(() => reports.Object);
        mock.Setup(x => x.Update(It.IsAny<Report>())).Returns((Report r) => r);
        mock.Setup(x => x.CreateAsync(It.IsAny<Report>())).ReturnsAsync((Report r) => r);
        mock.Setup(x => x.Remove(It.IsAny<Report>()));
        return mock;
    }

    public static Mock<IBaseRepository<User>> GetMockUserRepository()
    {
        var mock = new Mock<IBaseRepository<User>>();
        var users = GetUsers().BuildMockDbSet();
        mock.Setup(x => x.GetAll()).Returns(() => users.Object);
        mock.Setup(x => x.Update(It.IsAny<User>())).Returns((User u) => u);
        mock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);
        mock.Setup(x => x.Remove(It.IsAny<User>()));
        return mock;
    }

    public static IQueryable<Report> GetReports()
    {
        return new List<Report>
        {
            new Report
            {
                Id = 1,
                Name = "UnitTestReport1",
                Description = "UnitTestReport1",
                CreatedAt = DateTime.UtcNow,
                UserId = 1
            },
            new Report
            {
                Id = 2,
                Name = "UnitTestReport2",
                Description = "UnitTestReport2",
                CreatedAt = DateTime.UtcNow,
                UserId = 2
            },
        }.AsQueryable();
    }

    public static IQueryable<User> GetUsers()
    {
        return new List<User>
        {
            new User
            {
                Id = 1,
                Login = "UnitTestUser1",
                Password = "UnitTestPassword1",
            },
            new User
            {
                Id = 2,
                Login = "UnitTestUser2",
                Password = "UnitTestPassword2",
            },
        }.AsQueryable();
    }
}