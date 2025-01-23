using System.ComponentModel.DataAnnotations;
using Pedido.Models;

namespace Pedido.Dtos.Pedidos;

public class CreatePedidoDto {
    [Required] public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
    [Required] public string Status { get; set; }
}