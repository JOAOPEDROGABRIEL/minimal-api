using minimal_api.Infrastructure.Db;
using minimal_api.DTOs;
using minimal_api.Domain.Services;
using minimal_api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
static ErroValidacao ValidaADMDTO(AdministradorDTO administradorDTO)
{
    var VerifyError = new ErroValidacao {
        Msgs = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Email)) 
        VerifyError.Msgs.Add("O Campo 'Email' não pode estar vazio");
    if (string.IsNullOrEmpty(administradorDTO.Password))
        VerifyError.Msgs.Add("O Campo 'Password' não pode estar vazio");
    return VerifyError;    
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) => {
    if (administradorService.Login(loginDTO) != null) {
        return Results.Ok("Login Realizado com Sucesso");
    } else {
        return Results.Unauthorized();
    }
}).WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorService administradorService) => {
    var adm = administradorService.BuscaPorId(id);
    if (adm == null) return Results.NotFound();
    return Results.Ok(new AdmModelView{
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
        });
}).WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorService administradorService) => {
    var validacao = ValidaADMDTO(administradorDTO);
    if (validacao.Msgs.Count > 0)
        return Results.BadRequest(validacao);
    
    var administrador = new Administrador();
    administrador.Email = administradorDTO.Email;
    administrador.Password = administradorDTO.Password;
    administrador.Profile = administradorDTO.Profile ??= "editor";
    

    administradorService.Incluir(administrador);
    return Results.Created($"/administradores/{administrador.Id}", new AdmModelView{
            Id = administrador.Id,
            Email = administrador.Email,
            Profile = administrador.Profile
        });
}).WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? page, IAdministradorService administradorService) => {
    var admMV = new List<AdmModelView>();
    var adms = administradorService.Todos(page);

    foreach (var adm in adms)
    {
        admMV.Add(new AdmModelView{
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
        });
    }

    return Results.Ok(admMV);
}).WithTags("Administradores");
#endregion

#region Veiculos
static ErroValidacao ValidaDTO(VeiculoDTO veiculoDTO)
{
    var VerifyError = new ErroValidacao {
        Msgs = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Nome)) 
        VerifyError.Msgs.Add("O Campo 'Nome' não pode estar vazio");
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        VerifyError.Msgs.Add("O Campo 'Marca' não pode estar vazio");
    if (veiculoDTO.Ano < 1950)
        VerifyError.Msgs.Add("Veículo Muito Antigo, Aceito somente veículos superiores a 1950!");
    return VerifyError;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculoService) => {
    
    var validacao = ValidaDTO(veiculoDTO);
    if (validacao.Msgs.Count > 0)
         return Results.BadRequest(validacao);

    var veiculo = new Veiculo {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano };

    veiculoService.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? page, IVeiculoService veiculoService) => {
    var veiculos = veiculoService.Todos(page);
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) => {
    var veiculo = veiculoService.BuscaPorId(id);
    if (veiculo == null) return Results.NotFound();
    return Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoService veiculoService) => {
    var veiculo = veiculoService.BuscaPorId(id);
    if (veiculo == null || id == 0) return Results.NotFound();

    var validacao = ValidaDTO(veiculoDTO);
    if (validacao.Msgs.Count > 0)
         return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculoService.Atualizar(veiculo);

    return Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) => {
    var veiculo = veiculoService.BuscaPorId(id);
    if (veiculo == null) return Results.NotFound();
    veiculoService.Apagar(veiculo);
    return Results.NoContent();
}).WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion