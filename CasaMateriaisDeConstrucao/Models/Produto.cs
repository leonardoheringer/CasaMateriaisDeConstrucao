namespace CasaMateriaisDeConstrucao.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string? Categoria { get; set; }
        public string? ImagemUrl { get; set; }
    }
}