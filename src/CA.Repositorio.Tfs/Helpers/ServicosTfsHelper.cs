using CA.Core.Constantes;
using CA.Core.Entidades.Tfs;
using CA.Core.Valores;
using CA.Repositorios.Tfs.Extensions;
using CA.Repositorios.Tfs.Models;
using CA.Servicos.Tfs.Entidades;
using CA.Servicos.Tfs.Models.Responses;
using CA.Util.Helpers;
using System.Xml;
using System.Xml.Linq;

namespace CA.Repositorios.Tfs.Helpers
{
    internal class ServicosTfsHelper
    {
        internal static IEnumerable<ItemTrabalhoLink> ExtrairLinksItemTrabalhoDeResultado(QueryWorkitemsResponse resultado)
        {
            var lista = new List<ItemTrabalhoLink>();

            var origemId = 0;
            var destinoId = 0;
            var tipoLink = 0;
            var bloqueado = false;

            var relacionamentos = resultado.resultIds.SelectSingleNode("WorkItemLinkRelations");

            if (relacionamentos != null)
            {
                foreach (XmlElement filho in relacionamentos.ChildNodes)
                {
                    if (filho.Name.Equals("S", StringComparison.OrdinalIgnoreCase))
                    {
                        destinoId = 0;
                        tipoLink = 0;
                        bloqueado = false;

                        var i = XmlConvert.ToInt32(filho.GetAttribute("S"));
                        var num = i;

                        if (filho.HasAttribute("E"))
                        {
                            num = XmlConvert.ToInt32(filho.GetAttribute("E"));
                        }

                        for (; i <= num; i++)
                        {
                            lista.Add(new ItemTrabalhoLink(i, destinoId, tipoLink, bloqueado));
                        }

                        origemId = num;
                    }
                    else if (filho.Name.Equals("T", StringComparison.OrdinalIgnoreCase))
                    {
                        origemId = 0;
                        bloqueado = false;

                        var j = XmlConvert.ToInt32(filho.GetAttribute("S"));
                        var num2 = j;

                        if (filho.HasAttribute("E"))
                        {
                            num2 = XmlConvert.ToInt32(filho.GetAttribute("E"));
                        }

                        for (; j <= num2; j++)
                        {
                            lista.Add(new ItemTrabalhoLink(origemId, j, tipoLink, bloqueado));
                        }

                        destinoId = num2;
                    }
                    else
                    {
                        var atributo = filho.GetAttribute("S");

                        if (!string.IsNullOrEmpty(atributo))
                        {
                            origemId = XmlConvert.ToInt32(atributo);
                        }

                        var atributo2 = filho.GetAttribute("T");

                        if (!string.IsNullOrEmpty(atributo2))
                        {
                            destinoId = XmlConvert.ToInt32(atributo2);
                        }

                        var atributo3 = filho.GetAttribute("L");

                        if (!string.IsNullOrEmpty(atributo3))
                        {
                            tipoLink = XmlConvert.ToInt32(atributo3);
                        }

                        var atributo4 = filho.GetAttribute("E");

                        if (!string.IsNullOrEmpty(atributo4))
                        {
                            bloqueado = XmlConvert.ToInt32(atributo4) == 1;
                        }

                        lista.Add(new ItemTrabalhoLink(origemId, destinoId, tipoLink, bloqueado));
                    }
                }
            }

            var x = 0;

            lista.Sort(delegate (ItemTrabalhoLink a, ItemTrabalhoLink b)
            {
                var num = a.OrigemId - b.OrigemId;

                if (num == 0)
                {
                    num = a.DestinoId - b.DestinoId;
                }

                return -num;
            });

            foreach (var item in lista)
            {
                if (item.OrigemId != x)
                {
                    x = item.OrigemId;

                    yield return new ItemTrabalhoLink(0, item.OrigemId, 0, false);
                }
                if (item.DestinoId != 0)
                {
                    yield return new ItemTrabalhoLink(item.OrigemId, item.DestinoId, item.TipoLink, item.Bloqueado);
                }
            }
        }

        internal static IEnumerable<ItemTrabalho> ExtrairItemTrabalhoDeResultadoXml(PageWorkitemsByIdsResponse resultado, string colecao, IEnumerable<CampoTfs> camposSuportadosColecao)
        {
            var workitens = new List<ItemTrabalho>();

            var tabelas = resultado.items.Nodes.Descendants();

            var tabelaCamposItems = tabelas.FirstOrDefault(c =>
            {
                var atributo = c.Attribute("name");

                return atributo is not null && atributo.Value == "Items";
            });

            var tabelaCamposlongosItems = tabelas.FirstOrDefault(c =>
            {
                var atributo = c.Attribute("name");

                return atributo is not null && atributo.Value == "LongTextItems";
            });

            var tabelaRegistrosCampos = tabelaCamposItems is not null ? ExtrairRegistrosPorTabelaXml(tabelaCamposItems) : new TabelaRegistrosTfs();
            var tabelaRegistrosCamposLongos = tabelaCamposlongosItems is not null ? ExtrairRegistrosPorTabelaXml(tabelaCamposlongosItems) : new TabelaRegistrosTfs();

            foreach (var linha in tabelaRegistrosCampos.Linhas)
            {
                var workitem = new ItemTrabalho();

                workitem.Colecao = colecao;

                foreach (var campo in linha.Campos)
                {
                    var coluna = tabelaRegistrosCampos.ObterColunaPorIndice(campo.IndiceColuna);
                    var valor = campo.Valor;

                    if (coluna is null)
                        continue;

                    switch (coluna.Nome)
                    {
                        case NomesCamposTfs.Id: workitem.IdItemTrabalho = valor is not null ? (int)valor : 0; break;
                        case NomesCamposTfs.Revisao: workitem.Revisao = valor is not null ? (int)valor : 0; break;
                        case NomesCamposTfs.EstimativaOriginal: workitem.EstimativaOriginal = valor is not null ? (double)valor : 0; break;
                        case NomesCamposTfs.TempoConcluido: workitem.TempoConcluido = valor is not null ? (double)valor : 0; break;
                        case NomesCamposTfs.TempoRestante: workitem.TempoRestante = valor is not null ? (double)valor : 0; break;
                        case NomesCamposTfs.Titulo: workitem.Titulo = valor is not null ? (string)valor : string.Empty; break;
                        case NomesCamposTfs.IterationPath: workitem.IterationPath = valor is not null ? (string)valor : string.Empty; break;
                        case NomesCamposTfs.Projeto: workitem.Projeto = valor is not null ? (string)valor : string.Empty; break;
                        case NomesCamposTfs.Designado: workitem.Designado = valor is not null ? (string)valor : string.Empty; break;
                        case NomesCamposTfs.DataCriacao: workitem.DataCriacao = valor is not null ? (DateTime)valor : new DateTime(); break;
                        case NomesCamposTfs.TipoWorkitem: workitem.Tipo = valor is not null ? ItemTrabalhoHelper.TipoWorkitemTfsParaTipoItemTrabalho((string)valor) : TipoItemTrabalho.NaoReconhecido; break;
                        case NomesCamposTfs.Status: workitem.Status = valor is not null ? StatusHelper.StatusTfsParaStatusTarefa((string)valor) : StatusItemTrabalho.Desconhecido; break;
                    }
                }

                foreach (var linhaCampoLongo in tabelaRegistrosCamposLongos.Linhas)
                {
                    var colunaId = tabelaRegistrosCamposLongos.ObterColunaPorNome(NomesCamposTfs.IdCampo);
                    var colunaWorkItemId = tabelaRegistrosCamposLongos.ObterColunaPorNome(NomesCamposTfs.WorkItemId);
                    var colunaValorTextoLongo = tabelaRegistrosCamposLongos.ObterColunaPorNome(NomesCamposTfs.Words);

                    if (colunaId is null || colunaWorkItemId is null || colunaValorTextoLongo is null)
                        continue;

                    var valorIdCampo = linhaCampoLongo.ObterValorCampo(colunaId.Indice);
                    var valorIdWorkItem = linhaCampoLongo.ObterValorCampo(colunaWorkItemId.Indice);
                    var valorTextoLongo = linhaCampoLongo.ObterValorCampo(colunaValorTextoLongo.Indice);

                    if (valorIdWorkItem is null || (int)valorIdWorkItem != workitem.IdItemTrabalho)
                        continue;

                    var campoTagSuportado = camposSuportadosColecao.ObterCampoPorNomeReferencia(NomesCamposTfs.Tags);
                    var campoApotamentosSuportado = camposSuportadosColecao.ObterCampoPorNomeReferencia(NomesCamposTfs.Apontamentos);

                    if (campoTagSuportado is not null && valorIdCampo is not null && campoTagSuportado.Id == (int)valorIdCampo)
                    {
                        workitem.Tags = valorTextoLongo is not null ? (string)valorTextoLongo : string.Empty;
                    }
                    else if (campoApotamentosSuportado is not null && valorIdCampo is not null && campoApotamentosSuportado.Id == (int)valorIdCampo)
                    {
                        workitem.ListaApontamentos = valorTextoLongo is not null ? XmlHelper.DesserializarDeString<ListaApontamentos>((string)valorTextoLongo) : new ListaApontamentos();
                    }

                }

                workitens.Add(workitem);
            }

            return workitens;
        }

        internal static IEnumerable<CampoTfs> ExtrairCamposSuportadosDeResultadoXml(GetMetadataEx2Response resultado)
        {
            var camposSuportados = new List<CampoTfs>();

            var tabelas = resultado.metadata.Descendants();

            var tabelaCamposSuportados = tabelas.FirstOrDefault(c =>
            {
                var atributo = c.Attribute("name");

                return atributo is not null && atributo.Value == "Fields";
            });

            var tabelasRegistros = tabelaCamposSuportados is not null ? ExtrairRegistrosPorTabelaXml(tabelaCamposSuportados) : new TabelaRegistrosTfs();

            foreach (var linha in tabelasRegistros.Linhas)
            {
                var colunaId = tabelasRegistros.ObterColunaPorNome(NomesCamposTfs.IdCampo);
                var colunaNome = tabelasRegistros.ObterColunaPorNome(NomesCamposTfs.NomeReferencia);

                if (colunaId is null || colunaNome is null)
                    continue;

                var id = linha.ObterValorCampo(colunaId.Indice);
                var nome = linha.ObterValorCampo(colunaNome.Indice);

                camposSuportados.Add(new CampoTfs
                {
                    Id = id is not null ? (int)id : 0,
                    NomeReferencia = nome is not null ? nome.ToString() : string.Empty
                });
            }

            return camposSuportados
                        .GroupBy(c => c.Id)
                        .Select(c => c.First())
                        .ToList();
        }

        private static TabelaRegistrosTfs ExtrairRegistrosPorTabelaXml(XElement tabela)
        {
            var tabelaRegistros = new TabelaRegistrosTfs();

            if (tabela is null)
                return tabelaRegistros;

            var colunasXml = tabela.BuscarElementosDescedentesPorTag("columns").FirstOrDefault();
            var linhasXml = tabela.BuscarElementosDescedentesPorTag("rows").FirstOrDefault();

            if (colunasXml is null || linhasXml is null)
                return tabelaRegistros;

            var registroCamposXml = colunasXml.BuscarElementosDescedentesPorTag("c");
            var registroLinhasXml = linhasXml.BuscarElementosDescedentesPorTag("r");

            for (var i = 0; i < registroCamposXml.Count(); i++)
            {
                var nomeColunaXml = registroCamposXml.ElementAt(i).BuscarElementosDescedentesPorTag("n").First().Value;
                var tipoColunaXml = registroCamposXml.ElementAt(i).BuscarElementosDescedentesPorTag("t").First().Value;

                tabelaRegistros.AdicionarColuna(new ColunaRegistroTfs(i, nomeColunaXml, tipoColunaXml));
            }

            for (var i = 0; i < registroLinhasXml.Count(); i++)
            {
                var linha = new LinhaRegistrosTfs();

                var camposTfs = registroLinhasXml.ElementAt(i).BuscarElementosDescedentesPorTag("f").ToList();

                var indiceLinha = 0;
                var indiceColuna = 0;

                while (indiceLinha < camposTfs.Count())
                {
                    var atributoIndice = camposTfs[indiceLinha].Attribute("k");

                    indiceColuna = atributoIndice is not null ? int.Parse(atributoIndice.Value) : indiceColuna;

                    if (camposTfs.Count() > indiceLinha)
                    {
                        var coluna = tabelaRegistros.ObterColunaPorIndice(indiceColuna);

                        if (coluna is not null)
                        {
                            camposTfs[indiceLinha].Value.Parse(coluna.Tipo, out var valor);

                            linha.AdicionarCampo(new CampoRegistroTfs(valor, indiceColuna));
                        }
                    }

                    indiceLinha++;
                    indiceColuna++;
                }

                tabelaRegistros.AdicionarLinha(linha);
            }

            return tabelaRegistros;
        }

    }
}
