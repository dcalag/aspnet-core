using System;
using System.Collections.Generic;

namespace DcaLag.AspNet.Models {
    public class Usuario: BaseModel{
        public string Username { get; set;}

        public string Password { get; set; }

        public Boolean Enabled { get; set; }

        public int Id { get; set; }

        public List<UsuarioRol> Roles { get; set; }

        public Usuario() {
            Roles = new List<UsuarioRol>();
        }
    }
}