using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5000");
var app = builder.Build();

string connectionString = "Host=database_host;Port=5432;Database=first_task;Username=postgres;Password=postgres";

app.MapGet("/api/students", async () =>
{
    var students = new List<object>();
    await using var conn = new NpgsqlConnection(connectionString);
    await conn.OpenAsync();

    await using var cmd = new NpgsqlCommand("SELECT \"Id\", \"Name\", \"Surname\" FROM \"Students\"", conn);
    await using var reader = cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        students.Add(new
        {
            Id = reader.GetGuid(0).ToString(),
            Name = reader.GetString(1),
            Surname = reader.GetString(2)
        });
    }

    return Results.Ok(students);
});

app.Run();