
using Microsoft.EntityFrameworkCore;
using RequestFlow.Api.Data;
using RequestFlow.Api.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.MapGet("/api/health", () => {
    return new { status = "OK" };
});

app.MapGet("/api/tickets", async (AppDbContext db) =>
{
    var tickets = await db.Tickets.ToListAsync();

    return Results.Ok(tickets);
});

app.MapGet("/api/tickets/{id}", async (int id, AppDbContext db) =>
{
    var ticket = await db.Tickets.FindAsync(id);

    if (ticket == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(ticket);
});

app.MapPost("/api/tickets", async (CreateTicketRequest request, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.BadRequest("Title is required.");
    }
    var ticket = new SupportTicket
    {
        Title = request.Title,
        Description = request.Description,
        Priority = request.Priority,
        Status = "Open",
        CreatedAtUtc = DateTime.UtcNow
    };

    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    return Results.Created($"/api/tickets/{ticket.Id}", ticket);
});

app.Run();