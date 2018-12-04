using System;
using System.Collections.Generic;

namespace SistemaCadastro.Models.DB
{
    public partial class LoginByUsernamePassword
    {
        public int LoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
