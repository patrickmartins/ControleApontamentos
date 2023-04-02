using CA.Servicos.Channel.Constantes;
using CA.Servicos.Channel.Extensions;
using CA.Servicos.Channel.Models.Responses;
using ClosedXML.Excel;

namespace CA.Servicos.Channel.Helpers
{
    internal static class ChannelStreamResponseHelper
    {
        public static IEnumerable<ApontamentoResponse> ExtrairApontamentosDeRelatorio(Stream relatorio)
        {
            var apontamentos = new List<ApontamentoResponse>();

            using (var planilha = new XLWorkbook(relatorio))
            {
                var linhas = planilha.Worksheet(1).RowsUsed();                
                var cabecalho = linhas.FirstOrDefault();

                int colunaId, colunaUsuario, colunaProjeto, colunaAtividade, colunaData, colunaComentario, colunaTempo;

                if(cabecalho is not null)
                {
                    colunaId = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.Id);
                    colunaUsuario = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.IdColaborador);
                    colunaProjeto = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.IdProjeto);
                    colunaAtividade = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.Atividade);
                    colunaData = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.Data);
                    colunaComentario = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.Comentario);
                    colunaTempo = cabecalho.BuscarColunaPorValor(TitulosColunasApontamentos.Duracao);

                    if (colunaId <= 0)
                        throw new ApplicationException("A coluna 'Id' não foi encontrada na planilha de apontamentos.");

                    if (colunaUsuario <= 0)
                        throw new ApplicationException("A coluna 'Id Colaborador' não foi encontrada na planilha de apontamentos.");

                    if (colunaProjeto <= 0)
                        throw new ApplicationException("A coluna 'ID Projeto/ID Operação' não foi encontrada na planilha de apontamentos.");

                    if (colunaAtividade <= 0)
                        throw new ApplicationException("A coluna 'Atividade/Solicitação' não foi encontrada na planilha de apontamentos.");

                    if (colunaData <= 0)
                        throw new ApplicationException("A coluna 'Data' não foi encontrada na planilha de apontamentos.");

                    if (colunaComentario <= 0)
                        throw new ApplicationException("A coluna 'Comentário' não foi encontrada na planilha de apontamentos.");

                    if (colunaTempo <= 0)
                        throw new ApplicationException("A coluna 'Duração' não foi encontrada na planilha de apontamentos.");
                }
                else
                {
                    throw new ApplicationException("A planilha de apontamentos não contém uma linha de cabeçalho.");
                }

                foreach (var linha in linhas)
                {
                    if (linha.RowNumber() > 1 && !linha.Cell(1).Value.IsBlank)
                    {
                        var id = linha.Cell(colunaId).Value.IsText ? int.Parse(linha.Cell(colunaId).Value.GetText()) : (int)linha.Cell(colunaId).Value.GetNumber();
                        var idUsuario = linha.Cell(colunaUsuario).Value.IsText ? int.Parse(linha.Cell(colunaUsuario).Value.GetText()) : (int)linha.Cell(colunaUsuario).Value.GetNumber();
                        var idProjeto = linha.Cell(colunaProjeto).Value.IsText ? int.Parse(linha.Cell(colunaProjeto).Value.GetText()) : !linha.Cell(colunaProjeto).Value.IsBlank ? (int)linha.Cell(colunaProjeto).Value.GetNumber() : 0;

                        var atividade = linha.Cell(colunaAtividade).Value.GetText();
                        var data = linha.Cell(colunaData).Value.GetDateTime();
                        var comentario = linha.Cell(colunaComentario).Value.GetText();
                        var tempoApontado = !linha.Cell(colunaTempo).Value.IsBlank ? linha.Cell(colunaTempo).Value.GetDateTime().TimeOfDay : TimeSpan.Zero;

                        var codigoAtividade = atividade.Split(". ").Length > 1 ? atividade.Split(". ")[0] : string.Empty;
                        var nomeAtividade = atividade.Split(". ").Length > 1 ? string.Join(". ", atividade.Split(". ")[1..]) : string.Empty;

                        apontamentos.Add(new ApontamentoResponse
                        {
                            Id = id,
                            Data = data,
                            IdUsuario = idUsuario,
                            IdProjeto = idProjeto,
                            CodigoAtividade = codigoAtividade,
                            NomeAtividade = nomeAtividade,
                            Comentario = comentario,
                            TempoApontado = tempoApontado
                        });
                    }
                }
            }

            return apontamentos;
        }
    }
}
