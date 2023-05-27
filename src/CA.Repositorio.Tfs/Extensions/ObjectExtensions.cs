using System.Globalization;

namespace CA.Repositorios.Tfs.Extensions
{
    internal static class ObjectExtensions
    {
        private static readonly NumberFormatInfo _provider = new NumberFormatInfo();

        static ObjectExtensions()
        {
            _provider = new NumberFormatInfo();

            _provider.NumberDecimalSeparator = ".";
            _provider.NumberGroupSeparator = ",";
        }

        internal static bool Parse(this object obj, string typeName, out object result)
        {
            result = new object();

            if (obj == null)
                return false;

            var type = Type.GetType(typeName);

            if (type == null)
                return false;

            result = Convert.ChangeType(obj, type, _provider);

            return result != null;
        }
    }
}
