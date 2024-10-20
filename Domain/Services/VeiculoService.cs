using minimal_api.DTOs;
using minimal_api.Infrastructure.Db;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace minimal_api.Domain.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly DbContexto _contexto;

        public VeiculoService(DbContexto contexto)
        {
             _contexto = contexto;   
        }

        public void Apagar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public void Incluir(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public List<Veiculo> Todos(int page = 1, string? nome = null, string? marca = null)
        {
            var query = _contexto.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome}%"));
            }

            int itensPorPagina = 10;

            query = query.Skip((page - 1) * itensPorPagina).Take(itensPorPagina);
            return query.ToList();
        }
    }
}