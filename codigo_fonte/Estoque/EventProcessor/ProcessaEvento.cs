using System.Text.Json;
using AutoMapper;
using Estoque.Data;
using Estoque.Dtos.Pedido;
using Estoque.Models;

namespace Estoque.EventProcessor;

public class ProcessaEvento : IProcessaEvento {

    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _scopeFactory;

    public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory) {
        _mapper = mapper;
        _scopeFactory = scopeFactory;
    }

    void IProcessaEvento.Processa(string mensagem) {
        using var scope = _scopeFactory.CreateScope();

        var estoqueRepository = scope.ServiceProvider.GetRequiredService<IEstoqueRepository>();
        var readPedidoDto = JsonSerializer.Deserialize<ReadPedidoDto>(mensagem);

        var pedido = _mapper.Map<PedidoCliente>(readPedidoDto);

        /*Ainda estou sem o método ExistePedidoExterno nesse repository*/
        //if(!estoqueRepository.ExistePedidoExterno(pedido.PedidoKey)) {
            estoqueRepository.CreatePedido(pedido);
            estoqueRepository.SaveChanges();
        //}
    }
}