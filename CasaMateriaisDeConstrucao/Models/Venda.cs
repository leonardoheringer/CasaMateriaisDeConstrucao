namespace CasaMateriaisDeConstrucao.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public required Cliente Cliente { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal Total { get; set; }
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }

    public class ItemVenda
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public required Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}