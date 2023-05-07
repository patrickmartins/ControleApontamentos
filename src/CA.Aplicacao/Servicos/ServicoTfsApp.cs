using CA.Aplicacao.Extensions;
using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.Core.Valores;
using CA.Util.Extensions;

namespace CA.Aplicacao.Servicos
{
    public class ServicoTfsApp : IServicoTfsApp
    {
        private readonly IServicoTfs _servico;

        public ServicoTfsApp(IServicoTfs servico)
        {
            _servico = servico;
        }

        public async Task<Resultado<ApontamentoTfsModel>> AdicionarNovoApontamentoAsync(UsuarioTfs usuario, ApontamentoTfsNovoModel apontamentoModel)
        {
            if (apontamentoModel is null)
                return Resultado.DeErros<ApontamentoTfsModel>(new Erro("O apontamento não foi informado.", nameof(apontamentoModel)));

            if (apontamentoModel.Data.ConverterParaFusoBrasil().Date > DateTime.Now.ConverterParaFusoBrasil().Date)
                return Resultado.DeErros<ApontamentoTfsModel>(new Erro("A data informada deve ser menor ou igual a data atual.", nameof(apontamentoModel.Data)));

            var apontamento = ApontamentoTfsNovoModel.ViewModelParaApontamento(apontamentoModel);

            var resultado = await _servico.AdicionarNovoApontamentoAsync(usuario, apontamentoModel.Colecao, apontamentoModel.IdTarefa, ApontamentoTfsNovoModel.ViewModelParaApontamento(apontamentoModel));

            if (!resultado.Sucesso)
                return Resultado.DeErros<ApontamentoTfsModel>(resultado.Erros);

            return Resultado.DeValor(apontamento.ApontamentoTfsParaApontamentoTfsModel(apontamentoModel.IdTarefa));
        }

        public async Task<Resultado<Pagina<TarefaModel>>> BuscarTarefasAsync(UsuarioTfs usuario, string colecao, string palavraChave, StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10)
        {
            var resultado = await _servico.BuscarItensTrabalhoAsync(colecao, palavraChave, status, pagina, tamanhoPagina);

            if(!resultado.Sucesso)
                return Resultado.DeErros<Pagina<TarefaModel>>(resultado.Erros);

            var itens = resultado.Valor;

            return Resultado.DeValor(new Pagina<TarefaModel>
            {
                PalavraChave = itens.PalavraChave,
                TamanhoPagina = itens.TamanhoPagina,
                TotalResultados = itens.TotalResultados,
                NumeroPagina = itens.NumeroPagina,
                Resultados = ConsolidarTarefas(itens.Resultados).Select(c => c.ItemTrabalhoParaTarefaModel(usuario.NomeUsuario)).ToList()
            });
        }

        public async Task<Resultado<ApontamentosTfsDiaModel>> ObterApontamentosPorDiaAsync(UsuarioTfs usuario, DateOnly data)
        {
            var itensTrabalho = new List<ItemTrabalho>();

            foreach (var colecao in usuario.Colecoes)
            {
                var resultado = await _servico.ObterItensTrabalhoApontadosPorDatasAsync(usuario, colecao, data);

                if (!resultado.Sucesso)
                    return Resultado.DeErros<ApontamentosTfsDiaModel>(resultado.Erros);

                itensTrabalho.AddRange(resultado.Valor);
            }

            var apontamentos = ConsolidarTarefas(itensTrabalho).ItemTrabalhoParaApontamentoTfsDiaModel(usuario.NomeUsuario, data);

            return Resultado.DeValor(apontamentos);
        }

        public async Task<Resultado<ApontamentosTfsMesModel>> ObterApontamentosPorMesAsync(UsuarioTfs usuario, int mes, int ano)
        {
            var itensTrabalho = new List<ItemTrabalho>();

            if (mes < 1 || mes > 12)
                return Resultado.DeErros<ApontamentosTfsMesModel>(new Erro("O mês informado é inválido.", nameof(mes)));

            if (ano < 2000 || ano > DateTime.Today.Year)
                return Resultado.DeErros<ApontamentosTfsMesModel>(new Erro("O ano informado é inválido.", nameof(ano)));

            foreach (var colecao in usuario.Colecoes)
            {
                var resultado = await _servico.ObterItensTrabalhoApontadosPorPeriodoAsync(usuario, colecao, new DateOnly(ano, mes, 1), new DateOnly(ano, mes, (DateTime.Today.Month == mes && DateTime.Today.Year == ano) ? DateTime.Today.Day : DateTime.DaysInMonth(ano, mes)));

                if (!resultado.Sucesso)
                    return Resultado.DeErros<ApontamentosTfsMesModel>(resultado.Erros);

                itensTrabalho.AddRange(resultado.Valor);
            }

            var apontamentos = ConsolidarTarefas(itensTrabalho).ItemTrabalhoParaApontamentoTfsMesModel(usuario.NomeUsuario, mes, ano);

            return Resultado.DeValor(apontamentos);
        }

        public async Task<Resultado<IEnumerable<GrupoTarefasModel>>> ObterTarefasAtivasPorUsuarioAsync(UsuarioTfs usuario)
        {
            var itensTrabalho = new List<ItemTrabalho>();

            foreach (var colecao in usuario.Colecoes)
            {
                var resultado = await _servico.ObterItensTrabalhoPorUsuarioAsync(usuario, colecao);

                if (!resultado.Sucesso)
                    return Resultado.DeErros<IEnumerable<GrupoTarefasModel>>(resultado.Erros);

                itensTrabalho.AddRange(resultado.Valor);
            }

            var tarefas = ConsolidarTarefas(itensTrabalho).ItensTrabalhoParaItensAgrupadosModel(usuario.NomeUsuario);

            return Resultado.DeValor(tarefas);
        }

        public async Task<Resultado<IEnumerable<TarefaModel>>> ObterTarefasPorIdAsync(UsuarioTfs usuario, string colecao, params int[] ids)
        {
            var itensTrabalho = new List<ItemTrabalho>();

            var resultado = await _servico.ObterItensTrabalhoPorIdAsync(colecao, ids);

            if (!resultado.Sucesso)
                return Resultado.DeErros<IEnumerable<TarefaModel>>(resultado.Erros);

            itensTrabalho.AddRange(resultado.Valor);           

            var tarefas = ConsolidarTarefas(itensTrabalho).ItensTrabalhoParaTarefaModel(usuario.NomeUsuario);

            return Resultado.DeValor(tarefas);
        }

        public Task<Resultado<UsuarioTfs>> ObterUsuarioPorNomeAsync(string colecao, string nomeUsuario)
        {
            return _servico.ObterUsuarioPorNomeAsync(colecao, nomeUsuario);
        }

        private IEnumerable<ItemTrabalho> ConsolidarTarefas(IEnumerable<ItemTrabalho> itens)
        {
            return itens.Where(c => c.Tipo == TipoItemTrabalho.Tarefa || c.Tipo == TipoItemTrabalho.Bug).Select(item =>
            {
                var requisito = itens.Where(c => c.Tipo == TipoItemTrabalho.Requisito || c.Tipo == TipoItemTrabalho.HistoriaUsuario).FirstOrDefault(x => x.IdItemTrabalho == item.IdItemTrabalhoPai);

                item.Tags = requisito != null ? string.Join(";", requisito.Tags, item.Tags) : item.Tags;
                item.Tipo = DeterminarTipoDeItemTrabalho(item, requisito);

                return item;
            })
            .ToList();
        }

        private TipoItemTrabalho DeterminarTipoDeItemTrabalho(ItemTrabalho tarefa, ItemTrabalho? requisito)
        {
            if (tarefa.Tipo == TipoItemTrabalho.Bug)
                return TipoItemTrabalho.Bug;

            if (requisito != null && requisito.Titulo.Contains("[AD]"))
                return TipoItemTrabalho.Ad;

            if (tarefa.IterationPath.Contains("SUSTENTAÇÃO"))
                return TipoItemTrabalho.Chamado;

            return TipoItemTrabalho.Tarefa;
        }
    }
}
