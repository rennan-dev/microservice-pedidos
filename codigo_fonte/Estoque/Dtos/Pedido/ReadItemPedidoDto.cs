using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Estoque.Dtos.Pedido;

public class ReadItemPedidoDto {
    [JsonPropertyName("itemPedidoKey")]
    public int ItemPedidoKey { get; set; }
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("quantidade")]
    public int Quantidade { get; set; }
}