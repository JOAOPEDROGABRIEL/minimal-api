using minimal_api.Infrastructure.Db;
using minimal_api.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

app.MapGet("/", () => "Hello World!");

app.MapPost("Login", (minimal_api.DTOs.LoginDTO loginDTO) => {
    if (loginDTO.Email == "admin" && loginDTO.Password == "adm12345" ) {
        return Results.Ok("Login Realizado com Sucesso");
    } else {
        return Results.Unauthorized();
    }
});

app.Run();
