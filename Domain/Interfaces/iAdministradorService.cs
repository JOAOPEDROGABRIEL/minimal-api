using minimal_api.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces
{
    public interface IAdministradorService
    {
        Administrador? Login(LoginDTO loginDTO);
    }
}