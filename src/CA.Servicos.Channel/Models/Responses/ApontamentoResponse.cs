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
            return Regex.Matches(Comentario, "\\[[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\\]\\[\\d*\\] - \\[.*?\\] - ").Count == 1 
                && Regex.IsMatch(Comentario, "^\\[[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\\]\\[\\d*\\] - \\[.*?\\] - ");
        }

        public TipoApontamento ObterTipoDoApontamento()
        {
            return !string.IsNullOrEmpty(CodigoAtividade)
                                        ? TipoApontamento.Atividade
                                        : IdProjeto > 0 
                                                ? TipoApontamento.Projeto 
                                                : TipoApontamento.Avulso;
        }

        public int? ObterIdDaTarefaTfs()
        {
            if(!EhApontamentoTfs())
                return null;

            var resultado = Regex.Match(Comentario, "(?<=\\[[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\\]\\[)\\d*(?=\\] -)").Value;

            if (string.IsNullOrEmpty(resultado))
                return null;

            return int.TryParse(resultado, out var id) ? id : 0;
        }

        public string ObterIdDoApontamentoTfs()
        {
            if (!EhApontamentoTfs())
                return string.Empty;

            return Regex.Match(Comentario, "(?<=^\\[)[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}(?=\\])").Value;            
        }

        public string ObterComentarioDoApontamentoTfs()
        {
            if (!EhApontamentoTfs())
                return string.Empty;

            return Regex.Replace(Comentario, "^\\[[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\\]\\[\\d*\\] - \\[.*?\\] - ", string.Empty);
        }
    }
}
