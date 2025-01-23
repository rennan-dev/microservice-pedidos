using System.ComponentModel.DataAnnotations;
using Pedido.Models;

namespace Pedido.Dtos.Pedidos;

public class ReadPedidoDto {
    public int PedidoKey { get; set; }
    public List<ItemPedido> Itens { get; set; } 
    public string Status { get; set; }
}