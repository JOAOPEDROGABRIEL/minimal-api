using minimal_api.Infrastructure.Db;
using minimal_api.DTOs;
using minimal_api.Domain.Services;
using minimal_api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorService, AdministradorService>();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("Login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) => {
    if (administradorService.Login(loginDTO) != null) {
        return Results.Ok("Login Realizado com Sucesso");
    } else {
        return Results.Unauthorized();
    }
});

app.Run();
