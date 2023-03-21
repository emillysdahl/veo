public class Organization 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Employee Owner { get; set; }
}