using AutoMapper;
using Pedido.Models;
using Microsoft.AspNetCore.Mvc;
using Pedido.Data.PedidoRepository;
using Pedido.Dtos.Pedidos;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase {

    private readonly IPedidoRepository _repository;
    private readonly IMapper _mapper;

    public PedidoController(IPedidoRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
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
        var pedido = _mapper.Map<PedidoCliente>(createPedidoDto);
        _repository.CreatePedido(pedido);
        _repository.SaveChanges();

        var readPedidoDto = _mapper.Map<ReadPedidoDto>(pedido);

        return await Task.FromResult(CreatedAtRoute(nameof(GetPedidoById), new { id = readPedidoDto.PedidoKey }, readPedidoDto));
    }
}