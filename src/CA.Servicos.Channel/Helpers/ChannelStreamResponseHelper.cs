using CA.Servicos.Channel.Models.Responses;
using ClosedXML.Excel;
using System;

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

                foreach (var linha in linhas)
                {
                    if (linha.RowNumber() > 1 && !linha.Cell(1).Value.IsBlank)
                    {
                        var id = linha.Cell(1).Value.IsText ? int.Parse(linha.Cell(1).Value.GetText()) : (int)linha.Cell(1).Value.GetNumber();
                        var idUsuario = linha.Cell(3).Value.IsText ? int.Parse(linha.Cell(3).Value.GetText()) : (int)linha.Cell(3).Value.GetNumber();
                        var idProjeto = linha.Cell(8).Value.IsText ? int.Parse(linha.Cell(8).Value.GetText()) : !linha.Cell(8).Value.IsBlank ? (int)linha.Cell(8).Value.GetNumber() : 0;

                        var atividade = linha.Cell(10).Value.GetText();
                        var data = linha.Cell(2).Value.GetDateTime();
                        var comentario = linha.Cell(12).Value.GetText();
                        var tempoApontado = !linha.Cell(15).Value.IsBlank ? linha.Cell(15).Value.GetDateTime().TimeOfDay : TimeSpan.Zero;

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
