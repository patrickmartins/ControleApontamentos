namespace CA.Core.Entidades.CA
{
    public class Unidade
    {
        public string Id { get; set; }
        public string Nome { get; set; }

        public Unidade()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
