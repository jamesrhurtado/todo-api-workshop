using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

//creamos una instancia de la clase WebApplicationBuilder, que se utiliza para construir una aplicación web.
// CreateBuilder es un método estático que crea una instancia de WebApplicationBuilder y le pasa los argumentos proporcionados como parámetro.
// Los argumentos son opcionales y se utilizan para configurar la aplicación. Por ejemplo, se pueden utilizar para establecer la configuración de la aplicación, los servicios, el enrutamiento y otras características.
//es el punto de entrada para configurar una aplicación web en C# utilizando el marco de trabajo ASP.NET Core.
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("todos") ?? "Data Source=todos.db";

builder.Services.AddSqlite<TodoDb>(connectionString);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});


app.MapGet("/", () => "Bienvenidos!");

app.MapGet("/todos", async (TodoDb db) => await db.Todos.ToListAsync());
app.MapGet("/todos/{id}", async (TodoDb db, int id) => await db.Todos.FindAsync(id));


app.MapPost("/todos", async (TodoDb db, TodoItem todo) =>
{
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todo/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", async (TodoDb db, TodoItem updateTodo, int id) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Item = updateTodo.Item;
    todo.IsComplete = updateTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todos/{id}", async (TodoDb db, int id) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null)
    {
        return Results.NotFound();
    }
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();

    return Results.Ok(todo);
});

app.Run();

class TodoItem
{
    public int Id { get; set; }
    public string? Item { get; set; }
    public bool IsComplete { get; set; }
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions options) : base(options) { }
    public DbSet<TodoItem> Todos { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseInMemoryDatabase("Todos");
    // }
}


