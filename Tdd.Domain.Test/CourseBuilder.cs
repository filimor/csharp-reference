namespace Tdd.Domain.Test;

public class CourseBuilder
{
    private string _description = "Uma descrição";
    private string _name = "Informática básica";
    private double _price = 950d;
    private TargetAudience _publicoAlvo = TargetAudience.Student;
    private double _workload = 80d;

    public static CourseBuilder New()
    {
        return new CourseBuilder();
    }

    public CourseBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CourseBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CourseBuilder WithWorkload(double workload)
    {
        _workload = workload;
        return this;
    }

    public CourseBuilder WithValue(double price)
    {
        _price = price;
        return this;
    }

    public CourseBuilder WithTargetAudience(TargetAudience targetAudience)
    {
        _publicoAlvo = targetAudience;
        return this;
    }

    public Course Build()
    {
        return new Course(_name, _description, _workload, _publicoAlvo, _price);
    }
}
