using TodoApi;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Adding database context to DI container
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/get",async (TodoDb db) => { //Here also the lambda handler can get the param populated by DI Container
    var todos = await db.Todos.ToListAsync();
    return todos;
});
app.MapPost("/add", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    //Here Results is a factory for IResult which is
    // an interface that represents the result of an HTTP operation
    return Results.Created($"/get/{todo.Id}", todo);
   
});

app.Run();

