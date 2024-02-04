using books_api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

//connection db
var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
var connection = new NpgsqlConnection(connectionString);

builder.Services.AddScoped<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

var app = builder.Build();

//endpoints
app.MapPost("/create", async (HttpContext context) =>
{
    var bookFromBody = await context.Request.ReadFromJsonAsync<Book>();

    if (bookFromBody == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The request is missing data");
    } 
    else if (String.IsNullOrEmpty(bookFromBody.Title) 
    || String.IsNullOrEmpty(bookFromBody.Genre)
    || String.IsNullOrEmpty(bookFromBody.Author) 
    || String.IsNullOrEmpty(bookFromBody.Editor)
    || !DateTime.TryParse(bookFromBody.ReleaseDate.ToString(), out _))
        new BadRequestObjectResult("The request is missing data");

    var query = "INSERT INTO \"Books\" (\"Title\", \"Genre\", \"Author\", \"Editor\", \"ReleaseDate\") VALUES (@Title, @Genre, @Author, @Editor, @ReleaseDate) RETURNING \"Id\"";

    var parameters = new
    {
        Title = bookFromBody.Title,
        Genre = bookFromBody.Genre,
        Author = bookFromBody.Author,
        Editor = bookFromBody.Editor,
        ReleaseDate = bookFromBody.ReleaseDate,
    };

    var id = await connection.ExecuteScalarAsync<int>(query, parameters);
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync($"Book with id {id} saved succesfully");
});

// run app
app.Run();
