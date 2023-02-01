using CA.Core.Entidades.Channel;
using CA.Servicos.Channel.Models.Responses;
using CA.Util.Extensions;
using System.Net;

namespace CA.Jobs.Channel.Extensions
{
    internal static class ChannelResponseExtensions
    {
        public static UsuarioChannel ParaUsuarioChannel(this UsuarioResponse response)
        {
            return new UsuarioChannel
            {
                Id = response.Id,
                Email = response.Email.DecodificarHtml().Trim().Truncar(50).ParaUTF8().RemoverCaracteresNaoReconhecidos().RemoverQuebrasDeLinha(),
                NomeCompleto = response.Nome.DecodificarHtml().Trim().Truncar(200).ParaUTF8().RemoverCaracteresNaoReconhecidos().RemoverQuebrasDeLinha()
            };
        }

        public static IEnumerable<UsuarioChannel> ParaUsuariosChannel(this IEnumerable<UsuarioResponse> response)
        {
            return response.Select(c => c.ParaUsuarioChannel()).ToList();
        }

        public static ProjetoChannel ParaProjetoChannel(this ProjetoResponse response)
        {
            return new ProjetoChannel
            {
                Id = response.Id,
                Status = response.Status,
                Nome = response.Nome.DecodificarHtml().Trim().Truncar(200).ParaUTF8().RemoverCaracteresNaoReconhecidos().RemoverQuebrasDeLinha()
            };
        }

        public static IEnumerable<ProjetoChannel> ParaProjetosChannel(this IEnumerable<ProjetoResponse> response)
        {
            return response.Select(c => c.ParaProjetoChannel()).ToList();
        }

        public static AtividadeChannel ParaAtividadeChannel(this AtividadeResponse response, ProjetoChannel projeto)
        {
            return new AtividadeChannel
            {
                Id = response.Id,
                Nome = response.Nome.Trim().Truncar(200).ParaUTF8().RemoverCaracteresNaoReconhecidos().RemoverQuebrasDeLinha(),
                Codigo = response.Codigo.Trim().Truncar(30),
                Projeto = projeto
            };
        }

        public static IEnumerable<AtividadeChannel> ParaAtividadesChannel(this IEnumerable<AtividadeResponse> response, ProjetoChannel projeto)
        {
            return response.Select(c =>
            {
                if (projeto is null)
                    return new AtividadeChannel();

                return c.ParaAtividadeChannel(projeto);
            })
            .Where(c => c.Id > 0)
            .ToList();
        }

        public static ApontamentoChannel ParaApontamentoChannel(this ApontamentoResponse response, AtividadeChannel atividade, UsuarioChannel usuario)
        {
            return new ApontamentoChannel
            {
                Id = response.Id,
                Data = response.Data,
                Usuario = usuario,
                Atividade = atividade,
                Comentario = response.Comentario.Truncar(1000).ParaUTF8().RemoverCaracteresNaoReconhecidos().RemoverQuebrasDeLinha(),
                TempoApontado = response.TempoApontado
            };
        }

        public static IEnumerable<ApontamentoChannel> ParaApontamentosChannel(this IEnumerable<ApontamentoResponse> response, IEnumerable<AtividadeChannel> atividades, IEnumerable<UsuarioChannel> usuarios)
        {
            return response.Select(c =>
            {
                var atividade = atividades.FirstOrDefault(a => c.CodigoAtividade == a.Codigo && c.IdProjeto == a.Projeto.Id);
                var usuario = usuarios.FirstOrDefault(a => a.NomeCompleto.Equals(c.NomeUsuario.Trim()));

                if (usuario is null || atividade is null)
                    return new ApontamentoChannel();

                return c.ParaApontamentoChannel(atividade, usuario);
            })
            .Where(c => c.Id > 0)
            .ToList();
        }
    }
}
