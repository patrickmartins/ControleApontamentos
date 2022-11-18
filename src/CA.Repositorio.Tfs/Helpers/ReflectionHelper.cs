using System.Reflection;

namespace CA.Repositorios.Tfs.Helpers
{
    internal class ReflectionHelper
    {
        public static void AlterarPropriedade<TType>(TType objeto, string nomePropriedade, object valorPropriedade)
        {
            if (objeto == null) 
                return; 
            
            var propriedadeInfo = objeto.GetType().GetProperty(nomePropriedade, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty); 
            var valorConvertido = valorPropriedade;

            if (valorPropriedade != null && valorPropriedade.GetType() != propriedadeInfo.PropertyType)
            {
                var propertyType = Nullable.GetUnderlyingType(propriedadeInfo.PropertyType) ?? propriedadeInfo.PropertyType;

                valorConvertido = Convert.ChangeType(valorPropriedade, propertyType);
            }

            if (valorConvertido != null)
            {
                objeto.GetType().InvokeMember(nomePropriedade, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty, null, objeto, new object[] { valorPropriedade });
            }
        }
    }
}
