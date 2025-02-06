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

    /// <summary>
    /// Busca sincronamente todos os pedidos armazenados no projeto Pedido.
    /// </summary>
    /// <returns>Retorna todos os pedidos</returns>
    /// <response code="200">Pedidos localizados com sucesso.</response>
    /// <response code="400">Erro na requisição. Verifique os dados enviados.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<ReadPedidoDto>> GetAllPedidos() {
        var pedidos = _repository.GetAllPedidos();
        return Ok(_mapper.Map<IEnumerable<ReadPedidoDto>>(pedidos));
    }

    /// <summary>
    /// Busca sincronamente um pedido através do ID do projeto Pedido .
    /// </summary>
    /// <param name="id">Número inteiro para fazer a busca do pedido através do ID</param>
    /// <returns>Retorna o pedido se existir.</returns>
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
}