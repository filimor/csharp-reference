using System.Text.Json.Serialization;

namespace AdoNet.Models;

public class Contact
{
    public int Id { get; set; }

    [JsonIgnore] public int UserId { get; set; }

    public string PhoneNumber { get; set; }
    public string MobilePhone { get; set; }

    [JsonIgnore] public User User { get; set; }
}
