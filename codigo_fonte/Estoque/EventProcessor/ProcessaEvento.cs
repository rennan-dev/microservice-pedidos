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

    /*
    public void Processa(string mensagem) {
        using var scope = _scopeFactory.CreateScope();
        Console.WriteLine("Mensagem recebida: " + mensagem);
        var estoqueRepository = scope.ServiceProvider.GetRequiredService<IEstoqueRepository>();
        var readPedidoDto = JsonSerializer.Deserialize<ReadPedidoDto>(mensagem);
        if(readPedidoDto.Itens != null && readPedidoDto.Itens.Any()) {
            Console.WriteLine("Itens recebidos: " + readPedidoDto.Itens.Count());
        } else {
            Console.WriteLine("Nenhum item encontrado na mensagem");
        }

        var pedido = _mapper.Map<PedidoCliente>(readPedidoDto);

        Ainda estou sem o método ExistePedidoExterno nesse repository
        //if(!estoqueRepository.ExistePedidoExterno(pedido.PedidoKey)) {
            estoqueRepository.CreatePedido(pedido);
            estoqueRepository.SaveChanges();
        //}
    }*/
    public void Processa(string mensagem) {
        using var scope = _scopeFactory.CreateScope();

        var estoqueRepository = scope.ServiceProvider.GetRequiredService<IEstoqueRepository>();
        var readPedidoDto = JsonSerializer.Deserialize<ReadPedidoDto>(mensagem);

        // Log da mensagem recebida para depuração
        Console.WriteLine("Mensagem recebida: " + mensagem);

        if (readPedidoDto.Itens != null && readPedidoDto.Itens.Any()) {
            Console.WriteLine("Itens recebidos: " + readPedidoDto.Itens.Count());
        } else {
            Console.WriteLine("Nenhum item encontrado na mensagem");
        }

        var pedido = _mapper.Map<PedidoCliente>(readPedidoDto);

        // Verifique se os itens foram corretamente mapeados para o pedido
        Console.WriteLine("Itens no pedido mapeado: " + pedido.Itens.Count);

        estoqueRepository.CreatePedido(pedido);
        estoqueRepository.SaveChanges();
    }
}