using AutoMapper;
using Estoque.Data;
using Estoque.Dtos;
using Estoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase {

    private readonly IEstoqueRepository _repository;
    private readonly IMapper _mapper;

    public ProdutoController(IEstoqueRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Busca todos os produtos cadastrados.
    /// </summary>
    /// <returns>Retorna todos os produtos cadastrados.</returns>
    /// <response code="200">Produtos retornados com sucesso.</response>
    /// <response code="400">Erro na requisição. Verifique os dados enviados.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<ReadProdutoDto>> GetAllProdutos() {
        var produtos = _repository.GetAllProduto();
        return Ok(_mapper.Map<IEnumerable<ReadProdutoDto>>(produtos));
    }

    /// <summary>
    /// Busca um produto através do ID.
    /// </summary>
    /// <param name="id">Número inteiro para fazer a busca do produto através do ID</param>
    /// <returns>Retorna o produto se existir.</returns>
    /// <response code="200">Produto localizado com sucesso.</response>
    /// <response code="404">Produto não encontrado no database.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet("{id}", Name = "GetProdutoById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ReadProdutoDto> GetProdutoById(int id) {
        var produto = _repository.GetProdutoById(id);
        if(produto!=null) {
            return Ok(_mapper.Map<ReadProdutoDto>(produto));
        }
        return NotFound();
    }

    /// <summary>
    /// Adiciona um produto ao banco de dados.
    /// </summary>
    /// <param name="createProdutoDto">Objeto contendo os campos necessários para a criação de um produto.</param>
    /// <returns>Retorna o produto criado.</returns>
    /// <response code="201">Produto criado com sucesso.</response>
    /// <response code="400">Erro na requisição. Verifique os dados enviados.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadProdutoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReadProdutoDto>> CreateProduto(CreateProdutoDto createProdutoDto) {
        var produto = _mapper.Map<Produto>(createProdutoDto);
        _repository.CreateProduto(produto);
        _repository.SaveChanges();

        var readProdutoDto = _mapper.Map<ReadProdutoDto>(produto);

        return await Task.FromResult(CreatedAtRoute(nameof(GetProdutoById), new { id = readProdutoDto.ProductId }, readProdutoDto));
    }
}