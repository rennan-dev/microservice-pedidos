using AutoMapper;
using Estoque.Dtos;
using Estoque.Dtos.Pedido;
using Estoque.Models;
using Pedido.Dtos.Pedidos;

namespace Estoque.Profiles;

public class EstoqueProfile : Profile {
    public EstoqueProfile() {
        CreateMap<CreateProdutoDto, Produto>();
        CreateMap<Produto, ReadProdutoDto>();
        CreateMap<ReadProdutoDto, Produto>();

        //parte assíncrona - ItemPedido e Pedido
        CreateMap<ItemPedido, ReadItemPedidoDto>();
        CreateMap<PedidoCliente, ReadPedidoDto>();
        CreateMap<CreatePedidoDto, PedidoCliente>();
        CreateMap<CreateItemPedidoDto, ItemPedido>();
        CreateMap<ReadItemPedidoDto, ItemPedido>();
        CreateMap<ReadPedidoDto, PedidoCliente>().ForMember(destino=>destino.PedidoKey, opt=>opt.MapFrom(src=>src.PedidoKey ));
    }
}