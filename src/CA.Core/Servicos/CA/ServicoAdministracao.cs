using CA.Core.Entidades.CA;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.CA;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;

namespace CA.Core.Servicos.CA
{
    public class ServicoAdministracao : IServicoAdministracao
    {
        private readonly IRepositorioAdministracao _repositorioAdministracao;

        public ServicoAdministracao(IRepositorioAdministracao repositorioAdministracao)
        {
            _repositorioAdministracao = repositorioAdministracao;
        }

        public async Task<Resultado<Unidade>> AdicionarUnidadeAsync(Unidade unidade)
        {
            if (string.IsNullOrEmpty(unidade.Nome))
                return Resultado.DeErros<Unidade>(new Erro("O nome da unidade não foi informado.", nameof(unidade.Nome)));

            if (unidade.Nome.Length > 200)
                return Resultado.DeErros<Unidade>(new Erro("O nome da unidade não pode conter mais de 200 caracteres.", nameof(unidade.Nome)));

            await _repositorioAdministracao.InserirUnidadeAsync(unidade);
            await _repositorioAdministracao.SalvarAlteracoesAsync();

            return Resultado.DeValor(unidade);
        }

        public async Task<Resultado<Unidade>> AtualizarUnidadeAsync(Unidade unidade)
        {
            if (string.IsNullOrEmpty(unidade.Nome))
                return Resultado.DeErros<Unidade>(new Erro("O nome da unidade não foi informado.", nameof(unidade.Nome)));

            if (unidade.Nome.Length > 200)
                return Resultado.DeErros<Unidade>(new Erro("O nome da unidade não pode conter mais de 200 caracteres.", nameof(unidade.Nome)));

            _repositorioAdministracao.AtualizarUnidade(unidade);

            await _repositorioAdministracao.SalvarAlteracoesAsync();

            return Resultado.DeValor(unidade);
        }

        public Task<IEnumerable<Unidade>> ObterTodasUnidadesAsync()
        {
            return _repositorioAdministracao.ObterTodasUnidadesAsync();
        }

        public Resultado<Unidade?> ObterUnidadePorId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Resultado.DeErros<Unidade?>(new Erro("O id da unidade não foi informado.", nameof(id)));

            var unidade = _repositorioAdministracao.ObterUnidadePorId(id);

            if (unidade is null)
                return Resultado.DeErros<Unidade?>(new Erro("A unidade informada não foi encontrada.", nameof(id)));

            return Resultado.DeValor<Unidade?>(unidade);
        }
    }
}
