using CA.Core.Entidades.Channel;
using CA.Servicos.Channel.Models.Responses;
using CA.Util.Extensions;
using CA.Util.Helpers;
using System.Globalization;
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
            var projeto = new ProjetoChannel
            {
                Id = response.Id,
                Status = response.Status,
                Nome = response.Nome.DecodificarHtml().Trim().Truncar(200).ParaUTF8().RemoverCaracteresNaoReconhecidos().RemoverQuebrasDeLinha()                
            };

            projeto.Atividades = response.Atividades.ParaAtividadesChannel(projeto).ToList();

            return projeto;
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

        public static ApontamentoChannel ParaApontamentoChannel(this ApontamentoResponse response, ProjetoChannel projeto, UsuarioChannel usuario)
        {
            var atividade = projeto != null ? projeto.Atividades.FirstOrDefault(a => response.CodigoAtividade == a.Codigo && response.IdProjeto == a.Projeto.Id) : null;

            return new ApontamentoChannel
            {
                Hash = Sha1Helper.GerarHashPorString($"{response.ObterIdDaTarefaTfs()} - {response.ObterComentarioDoApontamentoTfs()} - {DateOnly.FromDateTime(response.Data)} - {response.TempoApontado}"),
                ApontamentoTfs = response.EhApontamentoTfs(),
                IdTarefaTfs = response.ObterIdDaTarefaTfs(),
                Tipo = response.ObterTipoDoApontamento(),
                Id = response.Id,
                Data = response.Data,
                Usuario = usuario,
                Projeto = projeto,
                Atividade = atividade,
                Comentario = response.Comentario,                
                TempoApontado = response.TempoApontado
            };
        }

        public static IEnumerable<ApontamentoChannel> ParaApontamentosChannel(this IEnumerable<ApontamentoResponse> response, IEnumerable<ProjetoChannel> projetos, IEnumerable<UsuarioChannel> usuarios)
        {
            return response.Select(c =>
            {
                var usuario = usuarios.FirstOrDefault(a => a.Id == c.IdUsuario);
                var projeto = projetos.FirstOrDefault(p => c.IdProjeto == p.Id);

                if (usuario is null)
                    return new ApontamentoChannel();

                var apontamento = c.ParaApontamentoChannel(projeto, usuario);

                return apontamento;
            })
            .Where(c => c.Id > 0)
            .ToList();
        }
    }
}
