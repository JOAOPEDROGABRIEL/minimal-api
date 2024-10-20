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
    }
}