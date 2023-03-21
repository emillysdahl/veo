using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("veo"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


// - To create a new organization
app.MapPost("/organization", async (Organization organization, DataContext db) =>
{
    db.Organizations.Add(organization);
    await db.SaveChangesAsync();

    return Results.Created($"/organization/{organization.Id}", organization);
});

// - To remove an organization
app.MapDelete("/organization/{id}", async (int id, DataContext db) =>
{
    if (await db.Organizations.FindAsync(id) is Organization organization)
    {
        db.Organizations.Remove(organization);
        await db.SaveChangesAsync();
        return Results.Ok(organization);
    }

    return Results.NotFound();
});

// - To get an organization by name
app.MapGet("/organization/name/{name}", (string name, DataContext db) =>
    db.Organizations.Where(o => o.Name == name).FirstOrDefault()
        is Organization organization
            ? Results.Ok(organization)
            : Results.NotFound());

// - To get an organization by id
app.MapGet("/organization/{id}", async (Guid id, DataContext db) =>
    await db.Organizations.FindAsync(id)
        is Organization organization
            ? Results.Ok(organization)
            : Results.NotFound());

// - To add employees to existing organization
// OBS! for this I assume that an organization has a list of employees (not specified in the task).
app.MapPost("/employee", async (Employee employee, DataContext db) =>
{
    var existingEmployee = await db.Employees.FindAsync(employee.Id);

    if (existingEmployee is not null) return Results.BadRequest("Employee already exists.");

    await db.SaveChangesAsync();

    return Results.Created($"/employee/{employee.Id}", employee);
});

// - To remove an employee from the organization
// OBS! for this I assume that an organization has a list of employees (not specified in the task).
app.MapDelete("/employee/{id}", async (int id, DataContext db) =>
{
    if (await db.Employees.FindAsync(id) is Employee employee)
    {
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return Results.Ok(employee);
    }

    return Results.NotFound();
});

// - To list all organizations
app.MapGet("/organization", async (DataContext db) =>
{
    var organizations = await db.Organizations.ToListAsync();

    return Results.Ok(organizations);
});
// - To list all the employees of organization
app.MapGet("/organization/{id}/employees", async (int id, DataContext db) =>
{
    var organization = await db.Organizations.FindAsync(id);
    var employees = organization.Employees;
    return Results.Ok(employees);
});

// - To list all resources of an employee
app.MapGet("/employee/{id}/resourses", async (int id, DataContext db) =>
{
    var employee = await db.Employees.FindAsync(id);
    var resources = employee.Resources;
    return Results.Ok(resources);
});

// - To get an employee by email

// - To get an employee by id
app.MapGet("/employee/{id}", async (int id, DataContext db) =>
    await db.Employees.FindAsync(id)
        is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

// - To update information of an employee
app.MapPut("/employee/{id}", async (int id, Employee inputEmployee, DataContext db) =>
{
    var existingEmployee = await db.Employees.FindAsync(id);

    if (existingEmployee is null) return Results.NotFound();

    existingEmployee.Name = inputEmployee.Name;
    existingEmployee.Title = inputEmployee.Title;
    existingEmployee.Department = inputEmployee.Department;
    existingEmployee.PhoneNumber = inputEmployee.PhoneNumber;
    existingEmployee.Email = inputEmployee.Email;
    existingEmployee.OwnerId = inputEmployee.OwnerId;
    existingEmployee.Resources = inputEmployee.Resources;
    existingEmployee.OrganizationId = inputEmployee.OrganizationId;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// - To update name or owner of the organization
app.MapPut("/organization/{id}/name", async (int id, string name, DataContext db) =>
{
    var existingEmployee = await db.Employees.FindAsync(id);

    if (existingEmployee is null) return Results.NotFound();

    existingEmployee.Name = name;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
