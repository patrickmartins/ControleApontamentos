using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.Core.Valores;
using CA.Util.Extensions;
using CA.Util.Helpers;

namespace CA.Core.Servicos.Tfs
{
    public class ServicoTfs : IServicoTfs
    {
        private readonly IRepositorioItensTrabalho _repositorioItens;
        private readonly IRepositorioUsuariosTfs _repositorioUsuarios;

        public ServicoTfs(IRepositorioItensTrabalho repositorioItens, IRepositorioUsuariosTfs repositorioUsuarios)
        {
            _repositorioItens = repositorioItens;
            _repositorioUsuarios = repositorioUsuarios;
        }

        public async Task<Resultado<UsuarioTfs>> ObterUsuarioPorNomeAsync(string usuario, string colecao)
        {
            if (string.IsNullOrEmpty(usuario))
                return Resultado.DeErros<UsuarioTfs>(new Erro("O usuário não foi informado.", nameof(usuario)));

            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<UsuarioTfs>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            var usuarioTfs = await _repositorioUsuarios.ObterUsuarioAsync(colecao, usuario);

            if (usuarioTfs is null)
                return Resultado.DeErros<UsuarioTfs>(new Erro("O usuário informado não foi encontrado.", nameof(usuario)));

            return Resultado.DeValor(usuarioTfs);
        }

        public async Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoPorIdAsync(string colecao, params int[] idsItemTrabalho)
        {
            if (!idsItemTrabalho.Any())
                return Resultado.DeValor<IEnumerable<ItemTrabalho>>(new ItemTrabalho[0]);

            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            var resultado = await _repositorioItens.ExecutarQueryAsync(colecao, $@"SELECT [System.Id]
                                                                                    FROM WorkItemLinks 
                                                                                    WHERE (Source.[System.WorkItemType] In ('Task','Bug')
                                                                                        and Source.[System.Id] in ({string.Join(",", idsItemTrabalho)}))
                                                                                    ORDER BY [System.Id] mode(MayContain)");

            var links = resultado.ToArray();

            return await ObterItemTrabalhoPorLinkAsync(colecao, links);
        }

        public async Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoPorUsuarioAsync(UsuarioTfs usuario, string colecao)
        {
            if (usuario is null)
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("O usuário não foi informado.", nameof(usuario)));

            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            var resultado = await _repositorioItens.ExecutarQueryAsync(colecao, $@"SELECT [System.Id]
                                                                                            FROM WorkItemLinks 
                                                                                            WHERE (Source.[System.WorkItemType] In ('Task','Bug')
                                                                                                and Source.[System.AssignedTo] = '{usuario.Dominio}\{usuario.NomeUsuario}' 
                                                                                                and Source.[System.State] In ('Active', 'Proposed'))
                                                                                                and (Target.[System.WorkItemType] In ('Requirement', 'User Story'))
                                                                                            ORDER BY [System.Id] mode(MayContain)");

            var links = resultado.ToArray();

            return await ObterItemTrabalhoPorLinkAsync(colecao, links);
        }

        public async Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoApontadosPorDataAsync(UsuarioTfs usuario, string colecao, DateOnly data)
        {
            if (usuario is null)
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("O usuário não foi informado.", nameof(usuario)));

            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            var resultado = await _repositorioItens.ExecutarQueryAsync(colecao, $@"SELECT [System.Id]
                                                                                    FROM WorkItemLinks 
                                                                                    WHERE (Source.[System.WorkItemType] In ('Task','Bug')                     
                                                                                        and Source.[Custom.Timesheets.TimesheetRawData] contains words 'CreatedBy=\""{usuario.NomeUsuario}\"" TimeSheetDate=\""{data:d}\""')" +
                                                                                    "ORDER BY [System.Id] mode(MayContain)");

            var links = resultado.ToArray();

            return await ObterItemTrabalhoPorLinkAsync(colecao, links);
        }

        public async Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoApontadosPorDatasAsync(UsuarioTfs usuario, string colecao, params DateOnly[] data)
        {
            if (usuario is null)
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("O usuário não foi informado.", nameof(usuario)));

            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<IEnumerable<ItemTrabalho>>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            var filtroPeriodo = string.Join(" or ", data.Select(c => $@"Source.[Custom.Timesheets.TimesheetRawData] contains words 'TimeSheetDate =\""{c.ToString("dd/MM/yyyy")}\""'"));

            var resultado = await _repositorioItens.ExecutarQueryAsync(colecao, $@"SELECT [System.Id]
                                                                                    FROM WorkItemLinks 
                                                                                    WHERE (Source.[System.WorkItemType] In ('Task','Bug')                     
                                                                                        and ({filtroPeriodo})) " +
                                                                                    "ORDER BY [System.Id] mode(MayContain)");

            var links = resultado.ToArray();

            return await ObterItemTrabalhoPorLinkAsync(colecao, links);
        }

        public Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoApontadosPorPeriodoAsync(UsuarioTfs usuario, string colecao, DateOnly inicio, DateOnly fim)
        {
            return ObterItensTrabalhoApontadosPorDatasAsync(usuario, colecao, DateTimeHelper.ObterIntervalo(inicio, fim).ToArray());
        }

        public async Task<Resultado<Pagina<ItemTrabalho>>> BuscarItensTrabalhoAsync(string colecao, string palavraChave, StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10)
        {
            if (string.IsNullOrEmpty(palavraChave))
                return new Resultado<Pagina<ItemTrabalho>>();

            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<Pagina<ItemTrabalho>>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            if (pagina <= 0)
                return Resultado.DeErros<Pagina<ItemTrabalho>>(new Erro("O número da página deve ser maior que 0.", nameof(pagina)));

            if (tamanhoPagina <= 0)
                return Resultado.DeErros<Pagina<ItemTrabalho>>(new Erro("O tamanho pagina da página deve ser maior que 0.", nameof(tamanhoPagina)));

            status = status.Where(c => c != StatusItemTrabalho.Desconhecido).ToArray();

            if (!status.Any())
                status = new StatusItemTrabalho[] { StatusItemTrabalho.Ativo };

            var filtroStatus = string.Join(',', status.Select(c => $"'{c.ObterDescricao()}'"));

            var resultadoLinks = await _repositorioItens.ExecutarQueryAsync(colecao, $@"SELECT [System.Id]
                                                                                            FROM WorkItemLinks 
                                                                                            WHERE (Source.[System.Title] contains words '{palavraChave}' 
                                                                                                or Source.[System.Description] contains words '{palavraChave}' 
                                                                                                or Source.[Microsoft.VSTS.TCM.ReproSteps] contains words '{palavraChave}'"
                                                                                                            + (palavraChave.ToArray().Any(c => !char.IsDigit(c)) ? string.Empty : $" or Source.[System.Id] = {palavraChave}") +
                                                                                                            @$") and (Source.[System.WorkItemType] In ('Task','Bug'))
                                                                                                and (Source.[System.State] In ({filtroStatus}))
                                                                                                and (Target.[System.WorkItemType] In ('Requirement', 'User Story'))
                                                                                                ORDER BY [System.Id] DESC mode(MayContain)");

            var links = resultadoLinks.ToArray();

            var totalResultados = links.Where(c => c.TipoLink == 0).Count();

            var tasksLinks = links.Where(c => c.TipoLink == 0).Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToArray();

            var ids = tasksLinks
                        .Select(c => c.DestinoId)
                        .Concat
                        (
                            links.Where(c => tasksLinks.Any(x => x.DestinoId == c.OrigemId)).Select(c => c.DestinoId)
                        )
                        .Distinct().ToArray();

            var tarefas = new List<ItemTrabalho>();

            if (ids.Any())
            {
                tarefas = (await _repositorioItens.ObterItensTrabalhoPorIdAsync(colecao, ids)).ToList();

                tarefas = ConfigurarLinks(tarefas, links).ToList();
            }

            return Resultado.DeValor(new Pagina<ItemTrabalho>
            {
                NumeroPagina = pagina,
                TamanhoPagina = tamanhoPagina,
                PalavraChave = palavraChave,
                TotalResultados = totalResultados,
                Resultados = tarefas
            });
        }

        public async Task<Resultado> AdicionarNovoApontamentoAsync(UsuarioTfs usuario, string colecao, int idItemTrabalho, Apontamento apontamento)
        {
            if (string.IsNullOrEmpty(colecao))
                return Resultado.DeErros<Resultado>(new Erro("A coleção do TFS não foi informada.", nameof(colecao)));

            if (idItemTrabalho <= 0)
                return Resultado.DeErros<Resultado>(new Erro("O id da tarefa não foi informado.", nameof(idItemTrabalho)));

            var itemTrabalho = (await _repositorioItens.ObterItensTrabalhoPorIdAsync(colecao, idItemTrabalho)).FirstOrDefault();

            if (itemTrabalho is null)
                return Resultado.DeErros<Resultado>(new Erro("O item de trabalho informado não foi encontrado.", nameof(idItemTrabalho)));

            if (itemTrabalho.Status == StatusItemTrabalho.Fechado)
                throw new ApplicationException("Não é possível fazer apontamentos em tarefas já fechadas.");

            itemTrabalho.ListaApontamentos.Apontamentos.Add(apontamento);

            var tempoTotalApontado = Math.Round(itemTrabalho.ListaApontamentos.ObterTempoTotalApontado().TotalHours, 2);
            var tempoRestante = CalcularTempoRestante(itemTrabalho.EstimativaOriginal, itemTrabalho.TempoRestante, tempoTotalApontado);

            itemTrabalho.TempoConcluido = tempoTotalApontado;
            itemTrabalho.TempoRestante = tempoRestante;

            await _repositorioItens.AtualizarItemTrabalhoAsync(usuario, itemTrabalho);

            return Resultado.DeSucesso<Resultado>();            
        }

        private async Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItemTrabalhoPorLinkAsync(string colecao, params ItemTrabalhoLink[] links)
        {
            if(!links.Any())
                return Resultado.DeValor<IEnumerable<ItemTrabalho>>(new ItemTrabalho[0]);

            var ids = links.Where(c => c.OrigemId > 0).Select(c => c.OrigemId).Concat(links.Select(c => c.DestinoId).ToArray()).ToArray();

            if (!ids.Any())
                return Resultado.DeValor<IEnumerable<ItemTrabalho>>(new ItemTrabalho[0]);

            var itensTrabalho = await _repositorioItens.ObterItensTrabalhoPorIdAsync(colecao, ids);

            return Resultado.DeValor(ConfigurarLinks(itensTrabalho, links));
        }

        private double CalcularTempoRestante(double estimativaInicial, double tempoRestante, double tempoTotal)
        {
            if (estimativaInicial > 0)
                tempoRestante = estimativaInicial >= tempoTotal ? estimativaInicial - tempoTotal : 0;
            else
                tempoRestante = tempoRestante >= tempoTotal ? tempoRestante - tempoTotal : 0;

            return Math.Round(tempoRestante >= 0 ? tempoRestante : 0, 2);
        }

        private IEnumerable<ItemTrabalho> ConfigurarLinks(IEnumerable<ItemTrabalho> itens, IEnumerable<ItemTrabalhoLink> links)
        {
            foreach(var item in itens) 
            {
                var itemPai = itens.FirstOrDefault(x => links.Any(c => c.OrigemId == item.IdItemTrabalho) && links.First(c => c.OrigemId == item.IdItemTrabalho).DestinoId == x.IdItemTrabalho);

                if (itemPai is not null)
                {
                    item.IdItemTrabalhoPai = itemPai.IdItemTrabalho;
                    item.TituloItemTrabalhoPai = itemPai.Titulo;
                }
            }

            return itens;
        }

    }
}