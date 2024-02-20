namespace Tdd.Domain;

public class Course
{
    public Course(string name, string description, double workload, TargetAudience targetAudience, double price)
    {
        Name = name;
        Description = description;
        Workload = workload;
        TargetAudience = targetAudience;
        Price = price;

        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        if (workload <= 0)
        {
            throw new Exception();
        }

        if (price <= 0)
        {
            throw new Exception();
        }
    }

    public string Name { get; }
    public string Description { get; }
    public double Workload { get; }
    public TargetAudience TargetAudience { get; }
    public double Price { get; }
}
