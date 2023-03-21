using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("veo"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapPost("/organization", async (Organization organization, DataContext db) =>
{
    db.Organizations.Add(organization);
    await db.SaveChangesAsync();

    return Results.Created($"/organization/{organization.Id}", organization);
});

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

app.MapGet("/organization/{name}", (string name, DataContext db) =>
    db.Organizations.Where(o => o.Name == name).FirstOrDefault()
        is Organization organization
            ? Results.Ok(organization)
            : Results.NotFound());

app.MapGet("/organization/{id}", async (int id, DataContext db) =>
    await db.Organizations.FindAsync(id)
        is Organization organization
            ? Results.Ok(organization)
            : Results.NotFound());

// app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return Results.NotFound();

//     todo.Name = inputTodo.Name;
//     todo.IsComplete = inputTodo.IsComplete;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });

// app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return Results.Ok(todo);
//     }

//     return Results.NotFound();
// });

app.Run();
