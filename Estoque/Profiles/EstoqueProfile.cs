using AutoMapper;
using Estoque.Dtos;
using Estoque.Models;

namespace Estoque.Profiles;

public class EstoqueProfile : Profile {
    public EstoqueProfile() {
        CreateMap<CreateProdutoDto, Produto>();
        CreateMap<Produto, ReadProdutoDto>();
        CreateMap<ReadProdutoDto, Produto>();
    }
}