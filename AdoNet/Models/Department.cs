using System.Text.Json.Serialization;

namespace AdoNet.Models;

public class Department
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [JsonIgnore] public ICollection<User>? Users { get; set; }
}
