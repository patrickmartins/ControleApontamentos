using CA.Core.Valores.Common;

namespace CA.Core.Valores
{
    public class Resultado : ResultadoBase
    {
        public Resultado(params Erro[] erros) : base(erros) { }
        public Resultado(List<Erro> erros) : base(erros) { }

        public static Resultado<TTipo> DeValor<TTipo>(TTipo valor) where TTipo : class?
        {
            return new Resultado<TTipo>(valor);
        }

        public static Resultado<TTipo> DeErros<TTipo>(params Erro[] erros) where TTipo : class?
        {
            return new Resultado<TTipo>(erros);
        }

        public static Resultado DeErros(params Erro[] erros)
        {
            return new Resultado(erros);
        }

        public static Resultado<TTipo> DeErros<TTipo>(List<Erro> erros) where TTipo : class?
        {
            return new Resultado<TTipo>(erros);
        }

        public static Resultado DeErros(List<Erro> erros)
        {
            return new Resultado(erros);
        }

        public static Resultado<TTipo> DeSucesso<TTipo>() where TTipo : class?
        {
            return new Resultado<TTipo>();
        }

        public static Resultado DeSucesso()
        {
            return new Resultado();
        }
    }

    public class Resultado<TEntidade> : Resultado where TEntidade : class?
    {
        public TEntidade Valor { get; private set; }

        public Resultado(params Erro[] erros) : base(erros) { }
        public Resultado(List<Erro> erros) : base(erros) { }

        public Resultado(TEntidade valor)
        {
            Valor = valor;
        }

        public void AtribuirValor(TEntidade valor)
        {
            Valor = valor;
        }
    }
}