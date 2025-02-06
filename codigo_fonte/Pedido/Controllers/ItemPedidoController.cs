using AutoMapper;
using Pedido.Models;
using Microsoft.AspNetCore.Mvc;
using Pedido.Data.ItemPedidoRepository;
using Pedido.Dtos.ItemPedidos;

namespace Estoque.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemPedidoController : ControllerBase {

    private readonly IItemPedidoRepository _repository;
    private readonly IMapper _mapper;

    public ItemPedidoController(IItemPedidoRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Busca todos os itensPedido armazenados no database.
    /// </summary>
    /// <returns>Retorna todos os itensPedidos</returns>
    /// <response code="200">itensPedido localizados com sucesso.</response>
    /// <response code="400">Erro na requisição. Verifique os dados enviados.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<ReadItemPedidoDto>> GetAllItensPedido() {
        var itensPedido = _repository.GetAllItemPedido();
        return Ok(_mapper.Map<IEnumerable<ReadItemPedidoDto>>(itensPedido));
    }

    /// <summary>
    /// Busca um item de pedido do através do ID.
    /// </summary>
    /// <param name="id">Número inteiro para fazer a busca do itemPedido através do ID</param>
    /// <returns>Retorna o itemPedido se existir.</returns>
    /// <response code="200">itemPedido localizado com sucesso.</response>
    /// <response code="404">itemPedido não encontrado no database.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpGet("{id}", Name = "GetItemPedidoById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ReadItemPedidoDto> GetItemPedidoById(int id) {
        var itemPedido = _repository.GetItemPedidoById(id);
        if(itemPedido!=null) {
            return Ok(_mapper.Map<ReadItemPedidoDto>(itemPedido));
        }
        return NotFound();
    }
}