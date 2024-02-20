using Bogus;
using ExpectedObjects;

namespace Tdd.Domain.Test;

public class CourseTest
{
    private readonly Faker _faker = new();

    [Fact]
    public void ShouldCreateCourse()
    {
        var expectedCourse = new
        {
            Name = _faker.Random.Word(),
            Workload = _faker.Random.Double(50, 1000),
            TargetAudience = TargetAudience.Student,
            Price = _faker.Random.Double(100, 1000),
            Description = _faker.Lorem.Paragraph()
        };

        var curso = new Course(expectedCourse.Name, expectedCourse.Description, expectedCourse.Workload,
            expectedCourse.TargetAudience, expectedCourse.Price);

        expectedCourse.ToExpectedObject().ShouldMatch(curso);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ShouldNotCourseHaveAnInvalidName(string invalidName)
    {
        Assert.Throws<Exception>(() =>
            CourseBuilder.New().WithName(invalidName).Build());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    [InlineData(-100)]
    public void ShouldNotCourseHaveAnInvalidWorkload(double invalidWorkload)
    {
        Assert.Throws<Exception>(() =>
                CourseBuilder.New().WithWorkload(invalidWorkload).Build())
            ;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    [InlineData(-100)]
    public void ShouldNotCourseHaveAnInvalidPrice(double invalidPrice)
    {
        Assert.Throws<Exception>(() =>
                CourseBuilder.New().WithValue(invalidPrice).Build())
            ;
    }
}
