using CasaMateriaisDeConstrucao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaMateriaisDeConstrucao.Services
{
    public interface IProdutoService
    {
        
    Task<List<Produto>> GetAllAsync();
    Task<Produto?> GetByIdAsync(int id); 

        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
    }
}