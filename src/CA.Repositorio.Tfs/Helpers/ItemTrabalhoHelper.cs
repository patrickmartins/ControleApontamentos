using CA.Core.Entidades.Tfs;

namespace CA.Repositorios.Tfs.Helpers
{
    internal class ItemTrabalhoHelper
    {
        public static TipoItemTrabalho TipoWorkitemTfsParaTipoItemTrabalho(string tipo)
        {
            switch (tipo.ToLower())
            {
                case "task": return TipoItemTrabalho.Tarefa;
                case "requirement": return TipoItemTrabalho.Requisito;
                case "bug": return TipoItemTrabalho.Bug;
                case "user story": return TipoItemTrabalho.HistoriaUsuario;
                case "new": return TipoItemTrabalho.Novo;
                default: return TipoItemTrabalho.NaoReconhecido;
            }
        }
    }
}
