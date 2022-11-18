namespace CA.Repositorios.Tfs.Extensions
{
    internal static class ObjectExtensions
    {
        internal static bool Parse(this object obj, string typeName, out object result)
        {
            result = new object();

            if (obj == null)
                return false;

            var type = Type.GetType(typeName);

            if (type == null)
                return false;

            result = Convert.ChangeType(obj, type);

            return result != null;
        }
    }
}
