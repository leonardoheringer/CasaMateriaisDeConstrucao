using CasaMateriaisDeConstrucao.Models;
using CasaMateriaisDeConstrucao.Data;
using Microsoft.EntityFrameworkCore;

namespace CasaMateriaisDeConstrucao.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProdutoService> _logger;

        public ProdutoService(ApplicationDbContext context, ILogger<ProdutoService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            try
            {
                return await _context.Produtos.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os produtos");
                throw;
            }
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Produtos.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produto por ID: {Id}", id);
                throw;
            }
        }

        public async Task AddAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            try
            {
                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();
                _context.Entry(produto).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto");
                throw;
            }
        }

        public async Task UpdateAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            try
            {
                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _context.Entry(produto).State = EntityState.Detached;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Conflito de concorrência ao atualizar produto: {Id}", produto.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto: {Id}", produto.Id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto != null)
                {
                    _context.Produtos.Remove(produto);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir produto: {Id}", id);
                throw;
            }
        }
    }
}