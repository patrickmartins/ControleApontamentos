using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Tfs;
using CA.Util.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CA.Aplicacao.Models
{
    public record ApontamentoTfsNovoModel
    {
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "O número da tarefa é inválido!")]
        public int IdTarefa { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "O tempo total da tarefa é inválido!")]
        public int TempoTotal { get; set; }

        [Required(ErrorMessage = "O comentário do apontamento não foi informado!")]
        public string Comentario { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "A coleção do TFS não foi informada!")]
        public string Colecao { get; set; }

        public static ApontamentoTfs ViewModelParaApontamento(ApontamentoTfsNovoModel viewModel)
        {
            return new ApontamentoTfs
            {
                SincronizadoChannel = false,
                Comentario = viewModel.Comentario,
                DataApontamento = DateTime.Now.ConverterParaFusoBrasil().ToString("d"),
                DataCriacao = DateTime.UtcNow,
                TempoApontamento = TimeSpan.FromMinutes(viewModel.TempoTotal).ToString("hh\\:mm"),
                Usuario = viewModel.Usuario
            };
        }
    }

    public abstract record ApontamentoModel
    {
        public string Hash { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
        public DateOnly Data { get; set; }
        public TimeSpan Tempo { get; set; }        
    }

    public record ApontamentoTfsModel : ApontamentoModel 
    {
        public bool SincronizadoChannel { get; set; }
    }

    public record ApontamentoChannelModel : ApontamentoModel
    {
        public int IdTarefaTfs { get; set; }
        public StatusApontamento Status { get; set; }
        public bool ApontamentoTfs { get; set; }
    }
}