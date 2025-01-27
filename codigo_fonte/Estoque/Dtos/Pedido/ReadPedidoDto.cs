using System.ComponentModel.DataAnnotations;
using Estoque.Models;

namespace Estoque.Dtos.Pedido;

public class ReadPedidoDto {
    public int PedidoKey { get; set; }
    public List<ReadItemPedidoDto> Itens { get; set; } 
    public string Status { get; set; }
}