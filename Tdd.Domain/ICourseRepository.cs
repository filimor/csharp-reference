namespace Tdd.Domain;

public interface ICourseRepository
{
    void Insert(Course course);
    Course GetByName(string courseDtoName);
}
