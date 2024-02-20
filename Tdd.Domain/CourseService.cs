namespace Tdd.Domain;

public class CourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public void Add(CourseDto courseDto)
    {
        if (!Enum.TryParse<TargetAudience>(courseDto.TargetAudience, out var targetAudience))
        {
            throw new Exception();
        }

        if (_courseRepository.GetByName(courseDto.Name) != null)
        {
            throw new Exception();
        }

        var course = new Course(courseDto.Name, courseDto.Description, courseDto.Workload, targetAudience,
            courseDto.Price);
        _courseRepository.Insert(course);
    }
}
