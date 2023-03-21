using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Organization 
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    [ForeignKey(nameof(Owner))]
    public int? OwnerId { get; set; }
    [JsonIgnore]
    public Employee? Owner { get; set; }
    public ICollection<Employee>? Employees { get; set; }
}