using Bogus;
using Moq;

namespace Tdd.Domain.Test;

public sealed class CourseServiceTest : IDisposable
{
    private readonly CourseDto _courseDto;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CourseService _courseService;
    private readonly Faker _faker = new();

    public CourseServiceTest()
    {
        _courseDto = new CourseDto
        {
            Name = _faker.Random.Word(),
            Workload = _faker.Random.Double(50, 1000),
            TargetAudience = "Student",
            Price = _faker.Random.Double(100, 1000),
            Description = _faker.Lorem.Paragraph()
        };
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _courseService = new CourseService(_courseRepositoryMock.Object);
    }

    public void Dispose()
    {
    }

    [Fact]
    public void ShouldAddCourse()
    {
        _courseService.Add(_courseDto);

        _courseRepositoryMock.Verify(r => r.Insert(It.Is<Course>(c =>
            c.Name == _courseDto.Name &&
            c.Description == _courseDto.Description &&
            c.TargetAudience == Enum.Parse<TargetAudience>(_courseDto.TargetAudience) &&
            Math.Abs(c.Workload - _courseDto.Workload) < 0.1 &&
            Math.Abs(c.Price - _courseDto.Price) < 0.01
        )), Times.Once);
    }

    [Fact]
    public void ShouldNotAcceptInvalidTargetAudience()
    {
        _courseDto.TargetAudience = "Médico";
        Assert.Throws<Exception>(() => _courseService.Add(_courseDto));
    }
}
