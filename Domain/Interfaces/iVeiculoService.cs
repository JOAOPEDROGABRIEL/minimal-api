using minimal_api.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces
{
    public interface IVeiculoService
    {
        List<Veiculo> Todos(int page = 1, string? nome = null, string? marca = null);
        Veiculo? BuscaPorId(int id);
        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }
}