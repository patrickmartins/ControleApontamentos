using CA.Aplicacao.Models;
using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;
using CA.Util.Extensions;
using CA.Util.Helpers;
using System.Text.RegularExpressions;

namespace CA.Aplicacao.Extensions
{
    internal static class EntidadeExtensions
    {
        private static readonly Regex _regexDesignado = new Regex("<[^>]*>");

        public static BatidasPontoDiaModel BatidasPontoParaBatidasPontoModel(this BatidasPontoDia batidaPonto)
        {
            var dataAtual = DateTime.Now.ConverterParaFusoBrasil();

            var totalTotalTrabalhado = batidaPonto.Saida1 > batidaPonto.Entrada1 ? batidaPonto.Saida1 - batidaPonto.Entrada1 : (batidaPonto.Entrada1.TotalMinutes != 0 && batidaPonto.Data.EHoje() ? new TimeSpan(dataAtual.Hour, DateTime.Now.Minute, 0) - batidaPonto.Entrada1 : TimeSpan.Zero);

            totalTotalTrabalhado += batidaPonto.Saida2 > batidaPonto.Entrada2 ? batidaPonto.Saida2 - batidaPonto.Entrada2 : (batidaPonto.Entrada2.TotalMinutes != 0 && batidaPonto.Data.EHoje() ? new TimeSpan(dataAtual.Hour, dataAtual.Minute, 0) - batidaPonto.Entrada2 : TimeSpan.Zero);
            totalTotalTrabalhado += batidaPonto.Saida3 > batidaPonto.Entrada3 ? batidaPonto.Saida3 - batidaPonto.Entrada3 : (batidaPonto.Entrada3.TotalMinutes != 0 && batidaPonto.Data.EHoje() ? new TimeSpan(dataAtual.Hour, dataAtual.Minute, 0) - batidaPonto.Entrada3 : TimeSpan.Zero);
            totalTotalTrabalhado += batidaPonto.Saida4 > batidaPonto.Entrada4 ? batidaPonto.Saida4 - batidaPonto.Entrada4 : (batidaPonto.Entrada4.TotalMinutes != 0 && batidaPonto.Data.EHoje() ? new TimeSpan(dataAtual.Hour, dataAtual.Minute, 0) - batidaPonto.Entrada4 : TimeSpan.Zero);
            totalTotalTrabalhado += batidaPonto.Saida5 > batidaPonto.Entrada5 ? batidaPonto.Saida5 - batidaPonto.Entrada5 : (batidaPonto.Entrada5.TotalMinutes != 0 && batidaPonto.Data.EHoje() ? new TimeSpan(dataAtual.Hour, dataAtual.Minute, 0) - batidaPonto.Entrada5 : TimeSpan.Zero);
            totalTotalTrabalhado += batidaPonto.Saida6 > batidaPonto.Entrada6 ? batidaPonto.Saida6 - batidaPonto.Entrada6 : (batidaPonto.Entrada6.TotalMinutes != 0 && batidaPonto.Data.EHoje() ? new TimeSpan(dataAtual.Hour, dataAtual.Minute, 0) - batidaPonto.Entrada6 : TimeSpan.Zero);

            return new BatidasPontoDiaModel
            {
                Entrada1 = batidaPonto.Entrada1,
                Entrada2 = batidaPonto.Entrada2,
                Entrada3 = batidaPonto.Entrada3,
                Entrada4 = batidaPonto.Entrada4,
                Entrada5 = batidaPonto.Entrada5,
                Entrada6 = batidaPonto.Entrada6,
                Saida1 = batidaPonto.Saida1,
                Saida2 = batidaPonto.Saida2,
                Saida3 = batidaPonto.Saida3,
                Saida4 = batidaPonto.Saida4,
                Saida5 = batidaPonto.Saida5,
                Saida6 = batidaPonto.Saida6,
                DataReferencia = batidaPonto.Data,
                TempoTotalTrabalhadoNoDia = totalTotalTrabalhado
            };
        }

        public static BatidasPontoMesModel BatidasPontoParaBatidasPontoMesModel(this IEnumerable<BatidasPontoDia> batidasDia, int mes, int ano)
        {
            var batidasPonto = batidasDia.Select(c => BatidasPontoParaBatidasPontoModel(c)).ToList();

            return new BatidasPontoMesModel
            {
                MesReferencia = mes,
                AnoReferencia = ano,
                BatidasDiarias = batidasPonto,
                TempoTotalTrabalhadoNoMes = TimeSpan.FromTicks(batidasPonto.Sum(c => c.TempoTotalTrabalhadoNoDia.Ticks))
            };
        }

        public static IEnumerable<TarefaModel> ItensTrabalhoParaTarefaModel(this IEnumerable<ItemTrabalho> itensTrabalho, string usuario)
        {
            return itensTrabalho.Select(c => c.ItemTrabalhoParaTarefaModel(usuario)).ToList();
        }

        public static TarefaModel ItemTrabalhoParaTarefaModel(this ItemTrabalho itemTrabalho, string usuario, DateOnly? dataReferencia = null)
        {
            var tarefa = new TarefaModel
            {
                Id = itemTrabalho.IdItemTrabalho,
                Tipo = itemTrabalho.Tipo,
                Titulo = itemTrabalho.Titulo,
                TituloPai = itemTrabalho.TituloItemTrabalhoPai,
                DataCriacao = itemTrabalho.DataCriacao,
                Colecao = itemTrabalho.Colecao,
                Designado = _regexDesignado.Replace(itemTrabalho.Designado, string.Empty).Trim(),
                Projeto = itemTrabalho.Projeto,
                Tags = itemTrabalho.Tags.Split(';', StringSplitOptions.RemoveEmptyEntries),
                Status = itemTrabalho.Status,
                ApontamentoHabilitado = itemTrabalho.ApontamentoHabilitado,                
                Apontamentos = itemTrabalho.
                                ListaApontamentos.Apontamentos.OrderByDescending(c => c.DataCriacao).Select(c =>
                                {
                                    DateOnly.TryParse(c.DataApontamento, out var data);
                                    TimeSpan.TryParse(c.TempoApontamento, out var tempo);

                                    return new ApontamentoModel
                                    {
                                        Data = data,
                                        Tempo = tempo,
                                        Comentario = c.Comentario.Trim(),
                                        Usuario = c.Usuario,
                                        SincronizadoChannel = c.SincronizadoChannel
                                    };
                                })
                                .ToList()
            };

            tarefa.TempoTotalApontadoSincronizadoChannel = new TimeSpan(tarefa.Apontamentos.Where(c => (c.Data == dataReferencia || dataReferencia is null) && c.Usuario.Equals(usuario) && c.SincronizadoChannel).Sum(c => c.Tempo.Ticks));
            tarefa.TempoTotalApontadoNaoSincronizadoChannel = new TimeSpan(tarefa.Apontamentos.Where(c => (c.Data == dataReferencia || dataReferencia is null) && c.Usuario.Equals(usuario) && !c.SincronizadoChannel).Sum(c => c.Tempo.Ticks));

            return tarefa;
        }

        public static AtividadeModel AtividadeChannelParaAtividadeModel(this AtividadeChannel atividade, DateOnly? dataReferencia = null)
        {
            var tarefa = new AtividadeModel
            {
                Id = atividade.Id,
                IdProjeto = atividade.Projeto.Id,
                NomeProjeto = atividade.Projeto.Nome,                
                Codigo= atividade.Codigo,
                Nome = atividade.Nome,
                Apontamentos = atividade.Apontamentos
                                        .OrderByDescending(c => c.Data)
                                        .Select(c => new ApontamentoModel
                                        {                                            
                                            Usuario = c.Usuario.Email.Split("@")[0],
                                            Data = DateOnly.FromDateTime(c.Data),
                                            Tempo = c.TempoApontado,
                                            Comentario = c.Comentario,
                                            SincronizadoChannel = true
                                        })
                                        .ToList()
            };

            tarefa.TempoTotalApontado = new TimeSpan(tarefa.Apontamentos.Where(c => (c.Data == dataReferencia || dataReferencia is null)).Sum(c => c.Tempo.Ticks));

            return tarefa;
        }

        public static IEnumerable<GrupoTarefasModel> ItensTrabalhoParaItensAgrupadosModel(this IEnumerable<ItemTrabalho> itensTrabalho, string usuario)
        {
            return new List<GrupoTarefasModel>
            {
                new GrupoTarefasModel
                {
                    Nome = "Atendimentos de Desenvolvimento",
                    Tarefas = itensTrabalho.Where(c => c.Tipo == TipoItemTrabalho.Ad).Select(c => c.ItemTrabalhoParaTarefaModel(usuario)).ToList()
                },

                new GrupoTarefasModel
                {
                    Nome = "Chamados",
                    Tarefas = itensTrabalho.Where(c => c.Tipo == TipoItemTrabalho.Chamado).Select(c => c.ItemTrabalhoParaTarefaModel(usuario)).ToList()
                },

                new GrupoTarefasModel
                {
                    Nome = "Bugs",
                    Tarefas = itensTrabalho.Where(c => c.Tipo == TipoItemTrabalho.Bug).Select(c => c.ItemTrabalhoParaTarefaModel(usuario)).ToList()
                },

                new GrupoTarefasModel
                {
                    Nome = "Tarefas",
                    Tarefas = itensTrabalho.Where(c => c.Tipo == TipoItemTrabalho.Tarefa).Select(c => c.ItemTrabalhoParaTarefaModel(usuario)).ToList()
                },

                new GrupoTarefasModel
                {
                    Nome = "Não Reconhecidos",
                    Tarefas = itensTrabalho.Where(c => c.Tipo == TipoItemTrabalho.NaoReconhecido).Select(c => c.ItemTrabalhoParaTarefaModel(usuario)).ToList()
                }
            };
        }

        public static ApontamentosChannelDiaModel AtividadeParaApontamentoChannelDiaModel(this IEnumerable<AtividadeChannel> atividades, DateOnly dataReferencia)
        {
            var atividadeModel = atividades
                                    .Where(c => c.Apontamentos.Any(a => DateOnly.FromDateTime(a.Data) == dataReferencia))
                                    .Select(c => c.AtividadeChannelParaAtividadeModel(dataReferencia)).ToList();

            return new ApontamentosChannelDiaModel
            {
                DataReferencia = dataReferencia,
                Atividades = atividadeModel,
                TempoTotalApontadoNoDia = new TimeSpan(atividadeModel.Select(c => c.TempoTotalApontado).Sum(c => c.Ticks))
            };
        }

        public static ApontamentosTfsDiaModel ItemTrabalhoParaApontamentoTfsDiaModel(this IEnumerable<ItemTrabalho> itensTrabalho, string usuario, DateOnly dataReferencia)
        {
            var itensTrabalhoViewModel = itensTrabalho
                                .Where(c => c.ListaApontamentos.Apontamentos.Any(a => a.Usuario.Equals(usuario) && a.DataApontamento.Equals(dataReferencia.ToString("d"))))
                                .Select(c => c.ItemTrabalhoParaTarefaModel(usuario, dataReferencia)).ToList();

            return new ApontamentosTfsDiaModel
            {
                DataReferencia = dataReferencia,
                Tarefas = itensTrabalhoViewModel,
                TempoTotalApontadoNoDia = new TimeSpan(itensTrabalhoViewModel.Select(c => c.TempoTotalApontadoNaoSincronizadoChannel + c.TempoTotalApontadoSincronizadoChannel).Sum(c => c.Ticks)),
                TempoTotalApontadoSincronizadoChannel = new TimeSpan(itensTrabalhoViewModel.Select(c => c.TempoTotalApontadoSincronizadoChannel).Sum(c => c.Ticks)),
                TempoTotalApontadoNaoSincronizadoChannel = new TimeSpan(itensTrabalhoViewModel.Select(c => c.TempoTotalApontadoNaoSincronizadoChannel).Sum(c => c.Ticks))
            };
        }

        public static ApontamentosChannelMesModel AtividadeParaApontamentoChannelMesModel(this IEnumerable<AtividadeChannel> atividades, int mes, int ano)
        {
            var apontamentosDiarios = DateTimeHelper.ObterIntervalo(new DateOnly(ano, mes, 1), new DateOnly(ano, mes, (DateTime.Today.Month == mes && DateTime.Today.Year == ano) ? DateTime.Today.Day : DateTime.DaysInMonth(ano, mes)))
                                                    .Select(c => atividades.AtividadeParaApontamentoChannelDiaModel(c))
                                                    .ToList();

            return new ApontamentosChannelMesModel
            {
                MesReferencia = mes,
                AnoReferencia = ano,
                ApontamentosDiarios = apontamentosDiarios.OrderBy(c => c.DataReferencia).ToList(),
                TempoTotalApontadoNoMes = new TimeSpan(apontamentosDiarios.Sum(c => c.TempoTotalApontadoNoDia.Ticks))                
            };
        }

        public static ApontamentosTfsMesModel ItemTrabalhoParaApontamentoTfsMesModel(this IEnumerable<ItemTrabalho> itensTrabalho, string usuario, int mes, int ano)
        {            
            var apontamentosDiarios = DateTimeHelper.ObterIntervalo(new DateOnly(ano, mes, 1), new DateOnly(ano, mes, (DateTime.Today.Month == mes && DateTime.Today.Year == ano) ? DateTime.Today.Day : DateTime.DaysInMonth(ano, mes)))
                                                    .Select(c => itensTrabalho.ItemTrabalhoParaApontamentoTfsDiaModel(usuario, c))
                                                    .ToList();

            return new ApontamentosTfsMesModel
            {
                MesReferencia = mes,
                AnoReferencia = ano,
                ApontamentosDiarios = apontamentosDiarios.OrderBy(c => c.DataReferencia).ToList(),
                TempoTotalApontadoNoMes = new TimeSpan(apontamentosDiarios.Sum(c => c.TempoTotalApontadoNoDia.Ticks)),
                TempoTotalApontadoSincronizadoChannel = new TimeSpan(apontamentosDiarios.Sum(c => c.TempoTotalApontadoSincronizadoChannel.Ticks)),
                TempoTotalApontadoNaoSincronizadoChannel = new TimeSpan(apontamentosDiarios.Sum(c => c.TempoTotalApontadoNaoSincronizadoChannel.Ticks)),
            };
        }
    }
}
