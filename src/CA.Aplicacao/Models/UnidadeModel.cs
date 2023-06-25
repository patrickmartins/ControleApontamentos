using System.ComponentModel.DataAnnotations;

namespace CA.Aplicacao.Models
{
    public record UnidadeModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome da unidade não pode conter mais de 200 caracteres.")]
        public string Nome { get; set; }
    }
}
