using System.Text.RegularExpressions;

namespace CA.Servicos.Channel.Models.Responses
{
    public class ApontamentoResponse
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string NomeUsuario { get; set; }
        public int IdProjeto { get; set; }
        public string CodigoAtividade { get; set; }
        public string NomeAtividade { get; set; }
        public string Comentario { get; set; }
        public TimeSpan TempoApontado { get; set; }

        public bool EhApontamentoTfs()
        {
            return Regex.IsMatch(Comentario, "^([\\d]+ - .* -)");
        }
    }
}
