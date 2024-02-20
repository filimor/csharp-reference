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
        var course = new Course(courseDto.Name, courseDto.Description, courseDto.Workload, courseDto.TargetAudience,
            courseDto.Price);
        _courseRepository.Insert(course);
    }
}
