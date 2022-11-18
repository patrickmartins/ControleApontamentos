using CA.Core.Entidades.Tfs;

namespace CA.Repositorios.Tfs.Helpers
{
    public static class StatusHelper
    {
        public static StatusItemTrabalho StatusTfsParaStatusTarefa(string status)
        {
            switch (status.ToLower())
            {
                case "active": return StatusItemTrabalho.Ativo;
                case "resolved": return StatusItemTrabalho.Resolvido;
                case "proposed": return StatusItemTrabalho.Proposto;
                case "closed": return StatusItemTrabalho.Fechado;
                default: return StatusItemTrabalho.Desconhecido;
            }
        }
    }
}