using System.ComponentModel;
using System.Reflection;

namespace CA.Util.Extensions
{
    public static class EnunExtensions
    {
        public static string ObterDescricao(this Enum e)
        {
            if (e is null)
                return string.Empty;

            var campo = e.GetType()
                            .GetTypeInfo()
                            .GetMember(e.ToString())
                            .FirstOrDefault(member => member.MemberType == MemberTypes.Field);

            if (campo is null)
                return e.ToString();

            var attribute = campo
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .SingleOrDefault()
                            as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }
    }
}
