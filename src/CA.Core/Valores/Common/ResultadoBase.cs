namespace CA.Core.Valores.Common
{
    public abstract class ResultadoBase
    {
        public List<Erro> Erros { get; private set; }
        public bool Sucesso => !Erros.Any();

        protected ResultadoBase(params Erro[] erros)
        {
            Erros = erros.ToList();
        }

        protected ResultadoBase(List<Erro> erros)
        {
            Erros = erros;
        }

        public ResultadoBase()
        {
            Erros = new List<Erro>();
        }

        public void AddError(string origem, string descricao)
        {
            Erros.Add(new Erro(descricao, origem));
        }

        public void AddError(Erro erro)
        {
            Erros.Add(erro);
        }

        public void AddErrors(IList<Erro> erros)
        {
            Erros = Erros.Concat(erros).ToList();
        }
    }
}
