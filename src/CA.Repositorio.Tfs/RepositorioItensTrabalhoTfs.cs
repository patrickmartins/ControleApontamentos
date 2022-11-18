using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.Repositorios.Tfs.Constantes;
using CA.Repositorios.Tfs.Entidades;
using CA.Repositorios.Tfs.Extensions;
using CA.Repositorios.Tfs.Helpers;
using CA.Repositorios.Tfs.Interfaces;
using CA.Repositorios.Tfs.ServicosSoap.Models;
using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using CA.Util.Helpers;

namespace CA.Repositorios.Tfs
{
    public class RepositorioItensTrabalhoTfs : IRepositorioItensTrabalho
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;
        private readonly IServicoWorkitemTfsSoap _servicoSoap;

        public RepositorioItensTrabalhoTfs(ConfiguracaoClienteTfs configuracoes, IServicoWorkitemTfsSoap servicoSoap)
        {
            configuracoes.Validar();

            _configuracoes = configuracoes;
            _servicoSoap = servicoSoap;            
        }

        public async Task<IEnumerable<ItemTrabalho>> ObterItensTrabalhoPorIdAsync(string colecao, params int[] ids)
        {
            var camposSuportados = await ObterCamposSuportadosPorColecaoAsync(colecao);

            var campoTag = camposSuportados.ObterCampoPorNomeReferencia(NomesCamposTfs.Tags);
            var campoApotamentos = camposSuportados.ObterCampoPorNomeReferencia(NomesCamposTfs.Apontamentos);

            var resultado = await _servicoSoap.ObterWorkitemsPorIdsAsync(new PageWorkitemsByIdsRequest
            {
                ids = ids,
                columns = new string[]
                {
                    NomesCamposTfs.Id,
                    NomesCamposTfs.TipoWorkitem,
                    NomesCamposTfs.Titulo,
                    NomesCamposTfs.IterationPath,
                    NomesCamposTfs.Projeto,
                    NomesCamposTfs.Designado,
                    NomesCamposTfs.Status,
                    NomesCamposTfs.DataCriacao,
                    NomesCamposTfs.Revisao,
                    NomesCamposTfs.DataAutorizacao,
                    NomesCamposTfs.DataAlteracao,
                    NomesCamposTfs.TempoConcluido,
                    NomesCamposTfs.TempoRestante,
                    NomesCamposTfs.EstimativaOriginal
                },
                longTextColumns = new int[]
                {
                    (campoTag is not null ? campoTag.Id : 0),
                    (campoApotamentos is not null ? campoApotamentos.Id : 0)
                }
            }
            , colecao);

            var itens = ServicosTfsHelper.ExtrairItemTrabalhoDeResultadoXml(resultado, colecao, camposSuportados);

           foreach(var item in itens)
                item.ApontamentoHabilitado = campoApotamentos != null;

            return itens;
        }

        public async Task<IEnumerable<ItemTrabalhoLink>> ExecutarQueryAsync(string colecao, string query)
        {
            var resultado = await _servicoSoap.ExecutarQueryAsync(new QueryWorkitemsRequest
                {
                    RequestHeader = new RequestHeader
                    {
                        Id = Guid.NewGuid().ToString(),
                        UseDisambiguatedIdentityString = 1
                    },
                    psQuery = XmlHelper.SerializarParaXmlElement(new Query
                    {
                        DayPrecision = true,
                        Wiql = query
                    }),
                    useMaster = false,
                    metadataHave = null
                }
                , colecao);

            var links = ServicosTfsHelper.ExtrairLinksItemTrabalhoDeResultado(resultado);

            return links;
        }

        private async Task<IEnumerable<CampoTfs>> ObterCamposSuportadosPorColecaoAsync(string colecao)
        {
            var resultado = await _servicoSoap.ObterMetadadosPorColecaoAsync(new GetMetadataEx2Request
            {
                metadataHave = new MetadataTableHaveEntry[]
                {
                    new MetadataTableHaveEntry
                    {
                        TableName = "Fields"
                    }
                },
                useMaster = true
            }
            , colecao);

            var campos = ServicosTfsHelper.ExtrairCamposSuportadosDeResultadoXml(resultado);

            return campos;
        }

        public async Task AtualizarItemTrabalhoAsync(UsuarioTfs usuario, ItemTrabalho itemTrabalho)
        {
            await _servicoSoap.AtualizarWorkitemAsync(new UpdateRequest
            {
                Package = new UpdateRequestPackage
                {
                    Package = new PackageUpdateWorkItem(itemTrabalho.Colecao, _configuracoes.UrlTfs)
                    {
                        UpdateWorkItem = new UpdateWorkItem("WorkItem", 0, itemTrabalho.IdItemTrabalho, itemTrabalho.Revisao)
                        {
                            InsertText = new UpdateField(NomesCamposTfs.Apontamentos)
                            {
                                Value = XmlHelper.SerializarParaString(itemTrabalho.ListaApontamentos)  
                            },
                            Columns = new List<Column> 
                            { 
                                new Column(NomesCamposTfs.AlteradoPor, nameof(String), usuario.NomeCompleto),
                                new Column(NomesCamposTfs.EstimativaOriginal, nameof(Double), itemTrabalho.EstimativaOriginal),
                                new Column(NomesCamposTfs.TempoConcluido, nameof(Double), itemTrabalho.TempoConcluido),
                                new Column(NomesCamposTfs.TempoRestante, nameof(Double), itemTrabalho.TempoRestante)
                            },
                            ComputedColumns = new List<ComputedColumn>
                            {
                                new ComputedColumn(NomesCamposTfs.PersonId),
                                new ComputedColumn(NomesCamposTfs.DataRevisao),
                                new ComputedColumn(NomesCamposTfs.DataAlteracao),
                                new ComputedColumn(NomesCamposTfs.DataAutorizacao),
                                new ComputedColumn(NomesCamposTfs.Watermark)
                            }
                        }
                    }
                }
            }
            , usuario
            , itemTrabalho.Colecao);            
        }
    }
}
