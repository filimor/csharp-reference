namespace Tdd.Domain;

public class CourseDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Workload { get; set; }
    public TargetAudience TargetAudience { get; set; }
    public double Price { get; set; }
}
