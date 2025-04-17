using CasaMateriaisDeConstrucao.Models;
using CasaMateriaisDeConstrucao.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CasaMateriaisDeConstrucao.Services
{
    public class VendaService : IVendaService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VendaService> _logger;
        private readonly IProdutoService _produtoService;

        public VendaService(
            ApplicationDbContext context, 
            ILogger<VendaService> logger,
            IProdutoService produtoService)
        {
            _context = context;
            _logger = logger;
            _produtoService = produtoService;
        }

        public async Task<List<Venda>> GetAllAsync()
        {
            return await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Venda?> GetByIdAsync(int id)
        {
            return await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Venda venda)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Atualiza o estoque para cada item da venda
                foreach (var item in venda.Itens)
                {
                    var produto = await _produtoService.GetByIdAsync(item.ProdutoId);
                    if (produto == null)
                    {
                        throw new Exception($"Produto com ID {item.ProdutoId} não encontrado");
                    }

                    if (produto.Estoque < item.Quantidade)
                    {
                        throw new Exception($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.Estoque}, Solicitado: {item.Quantidade}");
                    }

                    produto.Estoque -= item.Quantidade;
                    await _produtoService.UpdateAsync(produto);
                    
                    // Define o preço unitário no momento da venda
                    item.PrecoUnitario = produto.Preco;
                }

                // Calcula o total da venda
                venda.Total = venda.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
                venda.DataVenda = DateTime.Now;

                await _context.Vendas.AddAsync(venda);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Erro ao adicionar venda");
                throw;
            }
        }
    }
}