using books_api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;


namespace BooksApi.Endpoints;
public static class MinimalEndpoints
{
    public static void MapBookEndpoints(this WebApplication app, NpgsqlConnection connection)
    {
        app.MapPost("/create", async (context) =>
        {
            var bookFromBody = await context.Request.ReadFromJsonAsync<Book>();

            if (bookFromBody == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("The request is missing data");
            }
            else if (string.IsNullOrEmpty(bookFromBody.Title)
            || string.IsNullOrEmpty(bookFromBody.Genre)
            || string.IsNullOrEmpty(bookFromBody.Author)
            || string.IsNullOrEmpty(bookFromBody.Editor)
            || !DateTime.TryParse(bookFromBody.ReleaseDate.ToString(), out _))
                new BadRequestObjectResult("The request is missing data");

            var query = "INSERT INTO \"Books\" (\"Title\", \"Genre\", \"Author\", \"Editor\", \"ReleaseDate\") VALUES (@Title, @Genre, @Author, @Editor, @ReleaseDate) RETURNING \"Id\"";

            var parameters = new
            {
                bookFromBody.Title,
                bookFromBody.Genre,
                bookFromBody.Author,
                bookFromBody.Editor,
                bookFromBody.ReleaseDate,
            };

            var id = await connection.ExecuteScalarAsync<int>(query, parameters);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync($"Book with id {id} saved succesfully");
        });

        app.MapGet("get", async (context) =>
        {
            var bookId = context.Request.Query["bookId"];

            if (string.IsNullOrEmpty(bookId))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { Message = "Missing bookId parameter" });
            }

            var query = $"SELECT * FROM \"Books\" WHERE \"Id\" = '{bookId}'";
            var result = await connection.QueryFirstOrDefaultAsync<Book>(query);
            if (result != null)
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(new Book
                {
                    Title = result.Title,
                    Genre = result.Genre,
                    Author = result.Author,
                    Editor = result.Editor,
                    ReleaseDate = result.ReleaseDate
                });
            }
            else
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { Message = "Book not found" });
            }
        });

        app.MapDelete("delete", async (context) =>
        {
            var bookId = context.Request.Query["bookId"];

            if (string.IsNullOrEmpty(bookId))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { Message = "Missing bookId parameter" });
            }

            var deleteQuery = $"DELETE FROM \"Books\" WHERE \"Id\" = '{bookId}'";
            var rowsAffected = await connection.ExecuteAsync(deleteQuery);

            if (rowsAffected > 0)
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(new { Message = $"Book id {bookId} deleted sucessfully" });
            }
            else
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { Message = "An error has occurred trying to delete the book" });
            }
        });
    }
}
