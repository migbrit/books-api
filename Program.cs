using BooksApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
var connection = new NpgsqlConnection(connectionString);

builder.Services.AddScoped<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

var app = builder.Build();

MinimalEndpoints.MapBookEndpoints(app, connection);

app.Run();
