var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("Login", (minimal_api.DTOs.LoginDTO loginDTO) => {
    if (loginDTO.Email == "admin" && loginDTO.Password == "adm12345" ) {
        return Results.Ok("Login Realizado com Sucesso");
    } else {
        return Results.Unauthorized();
    }
});

app.Run();
