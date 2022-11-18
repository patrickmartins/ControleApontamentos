namespace CA.Core.Valores
{
    public class Erro
    {
        public string Descricao { get; private set; }
        public string Origem { get; private set; }

        public Erro(string descricao, string origem)
        {
            Descricao = descricao;
            Origem = origem;
        }
    }
}
