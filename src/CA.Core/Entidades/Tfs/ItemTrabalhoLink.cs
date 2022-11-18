namespace CA.Core.Entidades.Tfs
{
    public class ItemTrabalhoLink
    {
        public int OrigemId { get; set; }
        public int DestinoId { get; set; }
        public int TipoLink { get; set; }
        public bool Bloqueado { get; set; }


        public ItemTrabalhoLink(int origemId, int destinoId, int tipoLink, bool bloqueado)
        {
            if (origemId == 0 || destinoId == 0)
            {
                TipoLink = 0;
            }

            OrigemId = origemId;
            DestinoId = destinoId;
            Bloqueado = bloqueado;
            TipoLink = tipoLink;
        }
    }
}
