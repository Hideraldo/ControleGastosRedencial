using AutoMapper;
using ControleGastosRedencial.Server.Models;
using ControleGastosRedencial.Server.Models.Dtos;

namespace ControleGastosRedencial.Server.Mapping
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            var pessoaMap = CreateMap<Pessoa, PessoaDto>();
            pessoaMap.ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            var categoriaMap = CreateMap<Categoria, CategoriaDto>();
            categoriaMap.ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            var transacaoMap = CreateMap<Transacao, TransacaoDto>();
            transacaoMap.ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
