using Microsoft.Graph.Models;

namespace CA.Servicos.Graph.Extensions
{
    internal static class UserMicrosoftGraphExtensions
    {
        public static string ObterEmail(this User user)
        {
            if(string.IsNullOrEmpty(user.Mail))
                return string.Empty;

            return user.Mail;
        }

        public static string ObterNomeUsuario(this User user)
        {
            if (string.IsNullOrEmpty(user.Mail))
                return string.Empty;

            return user.Mail.Split("@")[0];
        }

        public static string ObterNomeCompleto(this User user)
        {
            if (string.IsNullOrEmpty(user.GivenName))
                return string.Empty;

            return $"{user.GivenName} {user.Surname}";
        }
    }
}
