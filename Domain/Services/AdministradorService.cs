using minimal_api.DTOs;
using minimal_api.Infrastructure.Db;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;

namespace minimal_api.Domain.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly DbContexto _contexto;

        public AdministradorService(DbContexto contexto)
        {
             _contexto = contexto;   
        }
        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
            return adm;
        }

        public Administrador Incluir(Administrador administrador)
        {
            _contexto.Administradores.Add(administrador);
            _contexto.SaveChanges();
            return administrador;
        }

        public List<Administrador> Todos(int? page = 1)
        {
            var query = _contexto.Administradores.AsQueryable();
           
            int itensPorPagina = 10;
            if (page != null)
            {
                query = query.Skip(((int)page - 1) * itensPorPagina).Take(itensPorPagina);
            }
            return query.ToList();
        }

        public Administrador? BuscaPorId(int id)
        {
            return _contexto.Administradores.Where(a => a.Id == id).FirstOrDefault();
        }
    }
}