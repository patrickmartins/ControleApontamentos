﻿using System.Security.Claims;
using System.Text.Json.Serialization;

namespace CA.Seguranca.Entidades
{
    public class UsuarioApp
    {
        public string NomeCompleto { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public bool PossuiContaPonto { get; set; }
        public bool PossuiContaTfs { get; set; }

        public ICollection<string> Colecoes { get; set; }
        public ICollection<string> Roles { get; set; }

        [JsonIgnore]
        public ICollection<Claim> Claims { get; set; }

        public UsuarioApp()
        {
            Colecoes = new HashSet<string>();
            Roles = new HashSet<string>();
            Claims = new HashSet<Claim>();
        }
    }
}
