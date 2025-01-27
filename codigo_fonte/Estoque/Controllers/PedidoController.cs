using AutoMapper;
using Estoque.Data;
using Estoque.Dtos.Pedido;
using Estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Pedido.Dtos.Pedidos;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase {

    private readonly IEstoqueRepository _repository;
    private readonly IMapper _mapper;

    public PedidoController(IEstoqueRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReadPedidoDto>> GetAllPedidos() {
        var pedidos = _repository.GetAllPedidos();
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
        var pedido = _mapper.Map<PedidoCliente>(createPedidoDto);
        
        _repository.CreatePedido(pedido);
        _repository.SaveChanges();

        var readPedidoDto = _mapper.Map<ReadPedidoDto>(pedido);
        if(readPedidoDto.Itens != null) {
            pedido.Itens = _mapper.Map<List<ItemPedido>>(readPedidoDto.Itens);
        }

        return await Task.FromResult(CreatedAtRoute(nameof(GetPedidoById), new { id = readPedidoDto.PedidoKey }, readPedidoDto));
    }
    
    /*
    [HttpPost]
    public async Task<ActionResult<ReadPedidoDto>> CreatePedido(CreatePedidoDto createPedidoDto) {
        // Mapeia o DTO para o modelo
        var pedido = _mapper.Map<PedidoCliente>(createPedidoDto);
        
        // Associa os itens ao pedido
        foreach (var item in pedido.Itens) {
            item.PedidoCliente = pedido; // Relaciona o item ao pedido
        }

        // Salva o pedido no repositório
        _repository.CreatePedido(pedido);
        _repository.SaveChanges();

        // Mapeia o modelo para o DTO de retorno
        var readPedidoDto = _mapper.Map<ReadPedidoDto>(pedido);

        // Retorna o resultado
        return await Task.FromResult(CreatedAtRoute(nameof(GetPedidoById), new { id = readPedidoDto.PedidoKey }, readPedidoDto));
    }
    */
    
}