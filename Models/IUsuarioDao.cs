using System.Collections.Generic;

namespace DcaLag.AspNet.Models{
    public interface IUsuarioDao{
        List<Usuario> RecuperarTodos();

        void Eliminar(int AUsuarioId);

        void ValidarUsuario(string AUserName, string APassword);

        List<string> RecuperarRoles(string AUserName);

        string ConnectionString { get; set; }

        void Guardar(Usuario AUsuario);

        Usuario Recuperar(int AId);
    }
}