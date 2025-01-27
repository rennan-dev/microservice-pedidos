using System.ComponentModel.DataAnnotations;

namespace Estoque.Dtos.Pedido;

public class ReadItemPedidoDto {
    public int ItemPedidoKey { get; set; }
    public int ProductId { get; set; }
    public int Quantidade { get; set; }
}