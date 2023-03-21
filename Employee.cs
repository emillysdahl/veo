public class Employee 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Department { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Employee Owner { get; set; }
    public ICollection<Employee> Resources { get; set; }
}