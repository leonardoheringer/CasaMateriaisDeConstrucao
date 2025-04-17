using CasaMateriaisDeConstrucao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaMateriaisDeConstrucao.Services
{
    public interface IVendaService
    {
        Task<List<Venda>> GetAllAsync();
        Task<Venda?> GetByIdAsync(int id);
        Task AddAsync(Venda venda);
    }
}