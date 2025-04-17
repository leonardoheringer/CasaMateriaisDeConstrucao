using CasaMateriaisDeConstrucao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaMateriaisDeConstrucao.Services
{
    public interface IClienteService
    {
      public interface IClienteService
{
    Task<List<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(int id); // Adicione o ?
    // ... outros métodos
}
        Task AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
    }
}