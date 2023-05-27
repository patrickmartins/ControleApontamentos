using CA.Aplicacao.Extensions;
using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Core.Valores;

namespace CA.Aplicacao.Servicos
{
    public class ServicoChannelApp : IServicoChannelApp
    {
        private readonly IServicoChannel _servico;

        public ServicoChannelApp(IServicoChannel servico)
        {
            _servico = servico;
        }

        public async Task<Resultado<ApontamentosChannelDiaModel>> ObterApontamentosPorDiaAsync(int idUsuario, DateOnly data)
        {
            var resultado = await _servico.ObterApontamentosPorDataAsync(idUsuario, data);

            if (!resultado.Sucesso)
                return Resultado.DeErros<ApontamentosChannelDiaModel>(resultado.Erros);

            var apontamentos = resultado.Valor.ApontamentosChannelParaApontamentoChannelDiaModel(data);

            return Resultado.DeValor(apontamentos);
        }

        public async Task<Resultado<ApontamentosChannelMesModel>> ObterApontamentosPorMesAsync(int idUsuario, int mes, int ano)
        {
            if (mes < 1 || mes > 12)
                return Resultado.DeErros<ApontamentosChannelMesModel>(new Erro("O mês informado é inválido.", nameof(mes)));

            if (ano < 2000 || ano > DateTime.Today.Year)
                return Resultado.DeErros<ApontamentosChannelMesModel>(new Erro("O ano informado é inválido.", nameof(ano)));

            var resultado = await _servico.ObterApontamentosPorPeriodoAsync(idUsuario, new DateOnly(ano, mes, 1), new DateOnly(ano, mes, (DateTime.Today.Month == mes && DateTime.Today.Year == ano) ? DateTime.Today.Day : DateTime.DaysInMonth(ano, mes)));

            if (!resultado.Sucesso)
                return Resultado.DeErros<ApontamentosChannelMesModel>(resultado.Erros);

            var apontamentos = resultado.Valor.ApontamentosChannelParaApontamentoChannelMesModel(mes, ano);

            return Resultado.DeValor(apontamentos);
        }

        public Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync()
        {
            return _servico.ObterTodosUsuariosAsync();
        }

        public Resultado<UsuarioChannel?> ObterUsuarioPorEmail(string emailUsuario)
        {
            return _servico.ObterUsuarioPorEmail(emailUsuario);
        }

        public Resultado<UsuarioChannel?> ObterUsuarioPorNomeCompleto(string nomeCompleto)
        {
            return _servico.ObterUsuarioPorNomeCompleto(nomeCompleto);
        }
    }
}
