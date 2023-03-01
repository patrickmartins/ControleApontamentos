using CA.Core.Entidades.Channel;
using System.Text.RegularExpressions;

namespace CA.Servicos.Channel.Models.Responses
{
    public class ApontamentoResponse
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int IdUsuario { get; set; }
        public int IdProjeto { get; set; }
        public string CodigoAtividade { get; set; }
        public string NomeAtividade { get; set; }
        public string Comentario { get; set; }
        public TimeSpan TempoApontado { get; set; }

        public bool EhApontamentoTfs()
        {
            return Regex.IsMatch(Comentario, "^\\[\\d*\\] - \\[.*\\] - ");
        }

        public TipoApontamento ObterTipoDoApontamento()
        {
            return !string.IsNullOrEmpty(CodigoAtividade)
                                        ? TipoApontamento.Atividade
                                        : IdProjeto > 0 
                                                ? TipoApontamento.Projeto 
                                                : TipoApontamento.Avulso;
        }

        public int ObterIdDaTarefaTfs()
        {
            if(!EhApontamentoTfs())
                return 0;

            var resultado = Regex.Match(Comentario, "(?<=^\\[)\\d*(?=\\])").Value;

            if (string.IsNullOrEmpty(resultado))
                return 0;

            return int.TryParse(resultado, out var id) ? id : 0;
        }

        public string ObterComentarioDoApontamentoTfs()
        {
            if (!EhApontamentoTfs())
                return string.Empty;

            return Regex.Replace(Comentario, "^\\[\\d*\\] - \\[.*\\] - ", string.Empty);
        }
    }
}
