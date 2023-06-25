using CA.Aplicacao.Enums;
using CA.Aplicacao.Extensions;
using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;
using CA.Core.Extensions;
using CA.Core.Interfaces.CA;
using CA.Core.Interfaces.Channel;
using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Core.Valores;

namespace CA.Aplicacao.Servicos
{
    public class ServicoRelatorioApp : IServicoRelatorioApp
    {
        private readonly IServicoChannel _servicoChannel;
        private readonly IServicoPonto _servicoPonto;
        private readonly IServicoTfs _servicoTfs;
        private readonly IServicoUsuariosCa _servicoUsuarioCa;

        public ServicoRelatorioApp(IServicoChannel servicoChannel, IServicoPonto servicoPonto, IServicoTfs servicoTfs, IServicoUsuariosCa servicoUsuarioCa)
        {
            _servicoChannel = servicoChannel;
            _servicoPonto = servicoPonto;
            _servicoTfs = servicoTfs;
            _servicoUsuarioCa = servicoUsuarioCa;
        }

        public async Task<Resultado<IEnumerable<RelatorioApontamentosUsuarioPorMes>>> ObterRelatorioDeApontamentosPorMesAsync(int mes, int ano, TipoOrdenacaoRelatorio ordenacao, bool somenteApontamentosSincronizados = false, bool somenteUsuariosComCadastroNoPonto = false, bool somenteUsuariosComTempoTrabalhado = false)
        {
            if (mes < 1 || mes > 12)
                return Resultado.DeErros<IEnumerable<RelatorioApontamentosUsuarioPorMes>>(new Erro("O mês informado é inválido.", nameof(mes)));

            if (ano < 2000 || ano > DateTime.Today.Year)
                return Resultado.DeErros<IEnumerable<RelatorioApontamentosUsuarioPorMes>>(new Erro("O ano informado é inválido.", nameof(ano)));

            var apontamentosTfsNaoSincronizados = new List<ItemTrabalho>();
            var apontamentosChannel = new List<ApontamentoChannel>();
            var batidasPonto = new List<BatidasPontoDia>();

            var dataInicio = new DateOnly(ano, mes, 1);
            var dataFim = new DateOnly(ano, mes, DateTime.DaysInMonth(ano, mes));

            var usuarios = (await _servicoUsuarioCa.ObterTodosUsuariosAsync()).Where(c => c.PossuiIntegracaoChannel() && (!somenteUsuariosComCadastroNoPonto || !somenteUsuariosComTempoTrabalhado || c.PossuiIntegracaoPonto())).ToList();

            if (!somenteApontamentosSincronizados) 
            {
                var colecoes = await _servicoTfs.ObterTodasColecoesAsync();

                foreach (var colecao in colecoes)
                {
                    var resultItensTrabalho = await _servicoTfs.ObterItensTrabalhoComApontamentosNaoSincronizadosAsync(colecao, dataInicio, dataFim);

                    if (!resultItensTrabalho.Sucesso)
                        return Resultado.DeErros<IEnumerable<RelatorioApontamentosUsuarioPorMes>>(resultItensTrabalho.Erros);

                    apontamentosTfsNaoSincronizados.AddRange(resultItensTrabalho.Valor);
                }
            }

            var resultApontamentosChannel = await _servicoChannel.ObterTodosApontamentosPorPeriodoAsync(dataInicio, dataFim);

            if (!resultApontamentosChannel.Sucesso)
                return Resultado.DeErros<IEnumerable<RelatorioApontamentosUsuarioPorMes>>(resultApontamentosChannel.Erros);

            apontamentosChannel = resultApontamentosChannel.Valor.ToList();

            var resultBatidasPonto = await _servicoPonto.ObterTodasBatidasPorPeriodoAsync(dataInicio, dataFim);

            if (!resultBatidasPonto.Sucesso)
                return Resultado.DeErros<IEnumerable<RelatorioApontamentosUsuarioPorMes>>(resultBatidasPonto.Erros);

            batidasPonto = resultBatidasPonto.Valor.ToList();

            var relatorioApontamentosMensais = new List<RelatorioApontamentosUsuarioPorMes>();

            foreach (var usuario in usuarios)
            {
                var usuarioTfs = usuario.ExtrairUsuarioTfs();
                var usuarioChannel = usuario.ExtrairUsuarioChannel();
                var funcionarioPonto = usuario.ExtrairFuncionarioPonto();

                var apontamentosUsuarioPorMes = new RelatorioApontamentosUsuarioPorMes
                {
                    Usuario = usuario.UsuarioCaParaUsuarioModel(),
                    AnoReferencia = ano,
                    MesReferencia = mes
                };

                if (usuarioTfs is not null)
                {
                    var apontamentosUsuarioTfs = apontamentosTfsNaoSincronizados.ItemTrabalhoParaApontamentoTfsMesModel(usuarioTfs.NomeUsuario, mes, ano);

                    apontamentosUsuarioPorMes.TempoTotalApontadoNaoSincronizadoNoTfsNoMes = apontamentosUsuarioTfs.TempoTotalApontadoNaoSincronizadoChannel;
                }

                if (usuarioChannel is not null)
                {
                    var apontamentosUsuarioChannel = apontamentosChannel.Where(c => c.Usuario.Id == usuarioChannel.Id).ApontamentosChannelParaApontamentoChannelMesModel(mes, ano);

                    apontamentosUsuarioPorMes.TempoTotalApontadoNoChannelNoMes = apontamentosUsuarioChannel.TempoTotalApontadoNoMes;
                }

                if(funcionarioPonto is not null)
                {
                    var batidasFuncionario = batidasPonto.Where(c => c.FuncionarioId == funcionarioPonto.Id).BatidasPontoParaBatidasPontoMesModel(mes, ano);

                    apontamentosUsuarioPorMes.TempoTotalTrabalhadoNoMes = batidasFuncionario.TempoTotalTrabalhadoNoMes;
                }

                apontamentosUsuarioPorMes.DiferencaTempoTrabalhadoApontado = new TimeSpan((apontamentosUsuarioPorMes.TempoTotalApontadoNoChannelNoMes.Ticks + apontamentosUsuarioPorMes.TempoTotalApontadoNaoSincronizadoNoTfsNoMes.Ticks) - apontamentosUsuarioPorMes.TempoTotalTrabalhadoNoMes.Ticks);

                relatorioApontamentosMensais.Add(apontamentosUsuarioPorMes);
            }

            relatorioApontamentosMensais = relatorioApontamentosMensais.Where(c => !somenteUsuariosComTempoTrabalhado || c.TempoTotalTrabalhadoNoMes.Ticks > 0).ToList();

            if(ordenacao == TipoOrdenacaoRelatorio.DiferencaHoras)
            {
                relatorioApontamentosMensais = relatorioApontamentosMensais.OrderByDescending(c => c.DiferencaTempoTrabalhadoApontado).ToList();
            }
            else
            {
                relatorioApontamentosMensais = relatorioApontamentosMensais.OrderBy(c => c.Usuario.NomeCompleto).ToList();
            }

            return Resultado.DeValor(relatorioApontamentosMensais.AsEnumerable());
        }
    }
}
