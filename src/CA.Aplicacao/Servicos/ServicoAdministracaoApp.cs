using CA.Aplicacao.Extensions;
using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Interfaces.CA;
using CA.Core.Valores;

namespace CA.Aplicacao.Servicos
{
    public class ServicoAdministracaoApp : IServicoAdministracaoApp
    {
        private readonly IServicoAdministracao _servicoAdministrador;
        private readonly IServicoUsuariosCa _servicoUsuarioCa;

        public ServicoAdministracaoApp(IServicoAdministracao servicoAdministrador, IServicoUsuariosCa servicoUsuarioCa)
        {
            _servicoAdministrador = servicoAdministrador;
            _servicoUsuarioCa = servicoUsuarioCa;
        }

        public async Task<Resultado<UnidadeModel>> AdicionarUnidadeAsync(UnidadeModel model)
        {
            var unidade = model.UnidadeModelParaUnidade();

            var resultado = await _servicoAdministrador.AdicionarUnidadeAsync(unidade);

            if(!resultado.Sucesso)
                return Resultado.DeErros<UnidadeModel>(resultado.Erros);

            return Resultado.DeValor(unidade.UnidadeParaUnidadeModel());
        }

        public async Task<Resultado<UnidadeModel>> AtualizarUnidadeAsync(UnidadeModel model)
        {
            var resultadoUnidade = _servicoAdministrador.ObterUnidadePorId(model.Id.ToString());

            if(!resultadoUnidade.Sucesso)
                return Resultado.DeErros<UnidadeModel>(resultadoUnidade.Erros);

            var unidade = resultadoUnidade.Valor;

            unidade.Nome = model.Nome;

            var resultado = await _servicoAdministrador.AtualizarUnidadeAsync(unidade);

            if (!resultado.Sucesso)
                return Resultado.DeErros<UnidadeModel>(resultado.Erros);

            return Resultado.DeValor(unidade.UnidadeParaUnidadeModel());
        }

        public async Task<IEnumerable<UnidadeModel>> ObterTodasUnidadesAsync()
        {
            return (await _servicoAdministrador.ObterTodasUnidadesAsync()).Select(c => c.UnidadeParaUnidadeModel());
        }

        public Resultado<UnidadeModel> ObterUnidadePorId(Guid id)
        {
            var resultadoUnidade = _servicoAdministrador.ObterUnidadePorId(id.ToString());

            if(!resultadoUnidade.Sucesso)
                return Resultado.DeErros<UnidadeModel>(resultadoUnidade.Erros);

            return Resultado.DeValor(resultadoUnidade.Valor.UnidadeParaUnidadeModel());
        }
    }
}
