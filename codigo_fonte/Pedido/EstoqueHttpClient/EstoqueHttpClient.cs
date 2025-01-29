using System.Text;
using System.Text.Json;
using Pedido.Dtos.Produto;

namespace Pedido.EstoqueHttpClient;

public class EstoqueHttpClient : IEstoqueHttpClient {

    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public EstoqueHttpClient(HttpClient client, IConfiguration configuration) {
        _client = client;
        _configuration = configuration;
    }
    public async Task<ReadProdutoDto> GetProdutoPorId(int productId) {
        var url = $"{_configuration["EstoqueService"]}/{productId}";

        Console.WriteLine($"Requisição para: {url}"); //log para depuração

        var response = await _client.GetAsync(url);

        if (response.IsSuccessStatusCode) {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ReadProdutoDto>(content);
        }

        Console.WriteLine($"Erro ao buscar produto. Status Code: {response.StatusCode}");
        return null;
    }
}