using AutoMapper;
using Pedido.Models;
using Microsoft.AspNetCore.Mvc;
using Pedido.Data.PedidoRepository;
using Pedido.Dtos.Pedidos;
using Pedido.EstoqueHttpClient;
using Pedido.RabbitMqClient;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase {

    private readonly IPedidoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEstoqueHttpClient _estoqueHttpClient;
    private IRabbitMqClient _rabbitMqClient;

    public PedidoController(IPedidoRepository repository, IMapper mapper, IEstoqueHttpClient estoqueHttpClient, IRabbitMqClient rabbitMqClient) {
        _repository = repository;
        _mapper = mapper;
        _estoqueHttpClient = estoqueHttpClient;
        _rabbitMqClient = rabbitMqClient;
    }

    /// <summary>
    /// Busca todos os Pedido armazenados no database.
    /// </summary>
    /// <returns>Retorna todos os Pedidos</returns>
    /// <response code="200">Pedidos localizados com sucesso.</response>
    /// <response code="400">Erro na requisição. Verifique os dados enviados.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<ReadPedidoDto>> GetAllPedido() {
        var pedidos = _repository.GetAllPedido();
        return Ok(_mapper.Map<IEnumerable<ReadPedidoDto>>(pedidos));
    }

    /// <summary>
    /// Busca um Pedido do através do ID.
    /// </summary>
    /// <param name="id">Número inteiro para fazer a busca do Pedido através do ID</param>
    /// <returns>Retorna o Pedido se existir.</returns>
    /// <response code="200">Pedido localizado com sucesso.</response>
    /// <response code="404">Pedido não encontrado no database.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet("{id}", Name = "GetPedidoById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ReadPedidoDto> GetPedidoById(int id) {
        var pedido = _repository.GetPedidoById(id);
        if(pedido!=null) {
            return Ok(_mapper.Map<ReadPedidoDto>(pedido));
        }
        return NotFound();
    }

    /// <summary>
    /// Cria assincronamente um novo pedido, envia para um mensageiro(rabbitmq) para que envie para o projeto Estoque.
    /// </summary>
    /// <param name="createPedidoDto">Objeto contendo os itens do pedido.</param>
    /// <returns>Retorna o pedido criado ou um erro caso não seja possível processá-lo.</returns>
    /// <response code="201">Pedido criado com sucesso.</response>
    /// <response code="400">Erro na requisição, como produto não encontrado ou quantidade maior que o estoque.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadPedidoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReadPedidoDto>> CreatePedido(CreatePedidoDto createPedidoDto) {
        bool pedidoCancelado = false;

        //usuário impedido de burlar e fazer o pedido do mesmo item várias vezes
        var itensAgrupados = createPedidoDto.Itens
            .GroupBy(i => i.ProductId)
            .Select(g => new {
                ProductId = g.Key,
                QuantidadeTotal = g.Sum(i => i.Quantidade)
            })
            .ToList();

        foreach(var item in itensAgrupados) {
            var produto = await _estoqueHttpClient.GetProdutoPorId(item.ProductId);

            if(produto == null) {
                return BadRequest($"O produto com ID {item.ProductId} não foi encontrado no estoque.");
            }

            //verifica se a quantidade pedida (já somada) é maior que a disponível
            if(item.QuantidadeTotal > produto.QuantidadeDisponivel) {
                pedidoCancelado = true;
            }
        }

        //mapear e criar o pedido
        var pedido = _mapper.Map<PedidoCliente>(createPedidoDto);
        pedido.Status = pedidoCancelado ? "Cancelado" : "Em análise";

        _repository.CreatePedido(pedido);
        _repository.SaveChanges();

        var readPedidoDto = _mapper.Map<ReadPedidoDto>(pedido);

        if(pedido.Status.Equals("Em análise")) {
            _rabbitMqClient.EnviarPedidoParaEstoque(readPedidoDto);
        }

        if(pedidoCancelado) {
            return BadRequest(new { mensagem = "Quantidade de pedido maior do que estoque", status = pedido.Status });
        }

        return CreatedAtRoute(nameof(GetPedidoById), new { id = readPedidoDto.PedidoKey }, readPedidoDto);
    }

    [HttpPut("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult UpdatePedido(int id, [FromBody] UpdatePedidoDto updatePedidoDto) {
        var pedidoExiste = _repository.GetPedidoById(id);
        
        if(pedidoExiste==null) {
            return NotFound();
        }

        pedidoExiste.Status = updatePedidoDto.Status;
        _repository.SaveChanges();

        return NoContent();
    }
}