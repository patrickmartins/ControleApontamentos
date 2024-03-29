﻿using CA.Core.Entidades.Ponto;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Ponto
{
    public interface IRepositorioPonto
    {
        Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim);
        Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterTodasBatidasPorPeriodoAsync(DateOnly inicio, DateOnly fim);
        Task<Resultado<BatidasPontoDia?>> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data);
        Task<Funcionario?> ObterFuncionarioPorIdAsync(int id);
        Task<Funcionario?> ObterFuncionarioPorNomeAsync(string nome);
        Task<Funcionario?> ObterFuncionarioPorPisAsync(string pisFuncionario);
        Task<IEnumerable<Funcionario>> ObterTodosFuncionariosAsync();
    }
}
