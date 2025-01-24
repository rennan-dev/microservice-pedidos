using AutoMapper;
using Pedido.Models;
using Microsoft.AspNetCore.Mvc;
using Pedido.Data.PedidoRepository;
using Pedido.Dtos.Pedidos;
using Pedido.EstoqueHttpClient;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase {

    private readonly IPedidoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEstoqueHttpClient _estoqueHttpClient;

    public PedidoController(IPedidoRepository repository, IMapper mapper, IEstoqueHttpClient estoqueHttpClient) {
        _repository = repository;
        _mapper = mapper;
        _estoqueHttpClient = estoqueHttpClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReadPedidoDto>> GetAllPedido() {
        var pedidos = _repository.GetAllPedido();
        return Ok(_mapper.Map<IEnumerable<ReadPedidoDto>>(pedidos));
    }

    [HttpGet("{id}", Name = "GetPedidoById")]
    public ActionResult<ReadPedidoDto> GetPedidoById(int id) {
        var pedido = _repository.GetPedidoById(id);
        if(pedido!=null) {
            return Ok(_mapper.Map<ReadPedidoDto>(pedido));
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<ReadPedidoDto>> CreatePedido(CreatePedidoDto createPedidoDto) {
        // Verificar se todos os produtos existem no Estoque
        foreach (var item in createPedidoDto.Itens) {
            var produto = await _estoqueHttpClient.GetProdutoPorId(item.ProductId);

            if (produto == null) {
                return BadRequest($"O produto com ID {item.ProductId} não foi encontrado no estoque.");
            }
        }

        // Mapear e criar o pedido
        var pedido = _mapper.Map<PedidoCliente>(createPedidoDto);
        _repository.CreatePedido(pedido);
        _repository.SaveChanges();

        var readPedidoDto = _mapper.Map<ReadPedidoDto>(pedido);

        return CreatedAtRoute(nameof(GetPedidoById), new { id = readPedidoDto.PedidoKey }, readPedidoDto);
    }
}