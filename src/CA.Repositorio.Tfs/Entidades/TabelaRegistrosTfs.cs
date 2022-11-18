namespace CA.Repositorios.Tfs.Entidades
{
    internal class TabelaRegistrosTfs
    {        
        public ICollection<ColunaRegistroTfs> Colunas { get; set; }
        public ICollection<LinhaRegistrosTfs> Linhas { get; set; }

        public TabelaRegistrosTfs()
        {
            Colunas = new List<ColunaRegistroTfs>();
            Linhas = new List<LinhaRegistrosTfs>();
        }

        public void AdicionarColuna(ColunaRegistroTfs coluna)
        {
            Colunas.Add(coluna);
        }

        public void AdicionarLinha(LinhaRegistrosTfs linha)
        {
            Linhas.Add(linha);
        }

        public ColunaRegistroTfs? ObterColunaPorIndice(int indice)
        {
            return Colunas.FirstOrDefault(c => c.Indice == indice);
        }

        public ColunaRegistroTfs? ObterColunaPorNome(string nome)
        {
            return Colunas.FirstOrDefault(c => c.Nome.Equals(nome));
        }

        public ICollection<CampoRegistroTfs> ObterCamposPorColuna(int indiceColuna)
        {
            return Linhas.SelectMany(c => c.Campos.Where(c => c.IndiceColuna == indiceColuna)).ToList();
        }
    }
}
