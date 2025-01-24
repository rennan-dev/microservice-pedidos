using System.ComponentModel.DataAnnotations;

namespace Pedido.Dtos.Produto;

public class ReadProdutoDto {
    public int ProductId { get; set; }
    public string Nome { get; set; }
    public int QuantidadeDisponivel { get; set; }
    public float Preco { get; set; }
}