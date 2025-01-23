using AutoMapper;
using Pedido.Dtos.Pedidos;
using Pedido.Models;

namespace Pedido.Profiles;

public class PedidoProfile : Profile {
    public PedidoProfile() {
        CreateMap<CreatePedidoDto, PedidoCliente>();
        CreateMap<PedidoCliente, ReadPedidoDto>();
        CreateMap<ReadPedidoDto, PedidoCliente>();
    }
}