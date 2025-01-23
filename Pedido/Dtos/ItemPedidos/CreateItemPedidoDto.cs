using System.ComponentModel.DataAnnotations;

namespace Pedido.Dtos.ItemPedidos;

public class CreateItemPedidoDto {
    [Required] public int ProductId { get; set; }
    [Required] public int Quantidade { get; set; }
}