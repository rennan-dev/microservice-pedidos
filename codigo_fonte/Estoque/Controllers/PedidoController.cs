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

        return await Task.FromResult(CreatedAtRoute(nameof(GetPedidoById), new { id = readPedidoDto.PedidoKey }, readPedidoDto));
    }
}