using System.Security.Cryptography;
using System.Text;

namespace CA.Util.Helpers
{
    public static class Sha1Helper
    {
        private static SHA1 _sha1;

        static Sha1Helper()
        {
            _sha1 = SHA1.Create();
        }

        public static string GerarHashPorString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            bytes = _sha1.ComputeHash(bytes);

            return Convert.ToHexString(bytes);
        }
    }
}
