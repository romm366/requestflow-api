using RequestFlow.Api.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var tickets = new List<SupportTicket>();

tickets.Add(new SupportTicket
{
    Id = 1,
    Title = "Monitor not working",
    Description = "The second monitor has no image.",
    Priority = "Medium",
    Status = "Open",
    CreatedAtUtc = DateTime.UtcNow
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/api/health", () => {
    return new { status = "OK" };
});

app.MapGet("/api/tickets", () => {
    return tickets;
});

app.MapGet("/api/tickets/{id}", (int id) =>
{
    var ticket = tickets.FirstOrDefault(t => t.Id == id);

    if (ticket == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(ticket);
});

app.MapPost("/api/tickets", (CreateTicketRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest("Title is required.");
    }
    var ticket = new SupportTicket
    {
        Id = tickets.Count + 1,
        Title = request.Title,
        Description = request.Description,
        Priority = request.Priority,
        Status = "Open",
        CreatedAtUtc = DateTime.UtcNow
    };

    tickets.Add(ticket);

    return Results.Created($"/api/tickets/{ticket.Id}", ticket);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
