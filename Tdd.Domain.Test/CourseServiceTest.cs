using Bogus;
using Moq;

namespace Tdd.Domain.Test;

public class CourseServiceTest
{
    private readonly Faker _faker = new();

    [Fact]
    public void ShouldAddCourse()
    {
        var courseDto = new CourseDto
        {
            Name = _faker.Random.Word(),
            Workload = _faker.Random.Double(50, 1000),
            TargetAudience = TargetAudience.Student,
            Price = _faker.Random.Double(100, 1000),
            Description = _faker.Lorem.Paragraph()
        };
        var courseRepositoryMock = new Mock<ICourseRepository>();
        var courseService = new CourseService(courseRepositoryMock.Object);

        courseService.Add(courseDto);

        courseRepositoryMock.Verify(x => x.Insert(It.Is<Course>(c =>
            c.Name == courseDto.Name &&
            c.Description == courseDto.Description &&
            c.TargetAudience == courseDto.TargetAudience &&
            Math.Abs(c.Workload - courseDto.Workload) < 0.1 &&
            Math.Abs(c.Price - courseDto.Price) < 0.01
        )), Times.Once);
    }
}
