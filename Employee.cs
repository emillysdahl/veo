using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Employee 
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Department { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    [ForeignKey(nameof(Owner))]
    public int? OwnerId { get; set; }
    [JsonIgnore]
    public Employee? Owner { get; set; }
    public ICollection<Employee>? Resources { get; set; }
    [ForeignKey(nameof(Organization))]
    public Guid? OrganizationId { get; set; }
    [JsonIgnore]
    public Organization? Organization { get; set; }
}