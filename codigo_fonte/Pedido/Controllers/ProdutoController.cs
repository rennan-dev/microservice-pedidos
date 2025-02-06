using System.Runtime.CompilerServices;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pedido.Data.ProdutoRepository;
using Pedido.Dtos.Produto;
using Pedido.EstoqueHttpClient;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase {

    private readonly IProdutoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEstoqueHttpClient _estoqueHttpClient;

    public ProdutoController(IProdutoRepository repository, IMapper mapper, IEstoqueHttpClient estoqueHttpClient) {
        _repository = repository;
        _mapper = mapper;
        _estoqueHttpClient = estoqueHttpClient;
    }

    /// <summary>
    /// Busca sincronamente todos os produtos cadastrados no projeto Estoque.
    /// </summary>
    /// <returns>Retorna todos os produtos cadastrados.</returns>
    /// <response code="200">Produtos retornados com sucesso.</response>
    /// <response code="400">Erro na requisição. Verifique os dados enviados.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ReadProdutoDto>>> GetProdutos() {
        var produtos = await _estoqueHttpClient.GetAllProdutos();  
        if(produtos == null || !produtos.Any()) {
            return NotFound("Nenhum produto encontrado.");
        }

        var produtosDto = _mapper.Map<IEnumerable<ReadProdutoDto>>(produtos);  
        return Ok(produtosDto); 
    }

    /// <summary>
    /// Busca um produto através do ID de forma síncrona com o projeto Estoque.
    /// </summary>
    /// <param name="id">Número inteiro para fazer a busca do produto através do ID</param>
    /// <returns>Retorna o produto se existir.</returns>
    /// <response code="200">Produto localizado com sucesso.</response>
    /// <response code="404">Produto não encontrado no database.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> VerificaProduto(int productId) {
        var produto = await _estoqueHttpClient.GetProdutoPorId(productId);
        if (produto == null) {
            return NotFound($"Produto com ID {productId} não encontrado no Estoque.");
        }

        Console.WriteLine($"Produto encontrado: {JsonSerializer.Serialize(produto)}");
        return Ok(produto);
    }
}