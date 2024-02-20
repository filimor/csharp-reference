namespace Tdd.Domain;

public class Course(string name, string description, double workload, TargetAudience targetAudience, double price)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public double Workload { get; } = workload;
    public TargetAudience TargetAudience { get; } = targetAudience;
    public double Price { get; } = price;
}
