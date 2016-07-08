using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace DcaLag.AspNet.Models.PG{
    public class UsuarioDaoPg : BaseDaoPg, IUsuarioDao 
    {
        public List<string> RecuperarRoles(string AUserName)
        {
            NpgsqlTransaction lTransaction = null;
            NpgsqlConnection lConn = null;
            var lRoles = new List<string>();

            try {
                lConn = new NpgsqlConnection(ConnectionString);
                lConn.Open();
                lTransaction = lConn.BeginTransaction(IsolationLevel.ReadCommitted);
                string lSql = "select role from user_roles where id_user = " +
                    "(select id from users where username = @username)";
                NpgsqlCommand lCmd = new NpgsqlCommand(lSql, lConn);
                lCmd.Parameters.AddWithValue("@username", AUserName);
                NpgsqlDataReader lReader = lCmd.ExecuteReader();

                while (lReader.Read())
                {
                    lRoles.Add(lReader.GetString(0));
                }
                    
                lReader.Close();
                lTransaction.Commit();
                lConn.Close();
            }
            catch{
                if (lTransaction != null)
                    lTransaction.Rollback();
                if (lConn != null)
                    lConn.Close();
            }

            return lRoles;
        }

        public void ValidarUsuario(string AUserName, string APassword)
        {
            NpgsqlTransaction lTransaction = null;
            NpgsqlConnection lConn = null;
            var lExiste = false;

            try{
                lConn = new NpgsqlConnection(ConnectionString);
                lConn.Open();
                lTransaction = lConn.BeginTransaction(IsolationLevel.ReadCommitted);
                string lSql = "select * from users where username=@username and password=@password";
                NpgsqlCommand lCmd = new NpgsqlCommand(lSql, lConn);
                lCmd.Parameters.AddWithValue("@username", AUserName);
                lCmd.Parameters.AddWithValue("@password", APassword);
                NpgsqlDataReader lReader = lCmd.ExecuteReader();

                while (lReader.Read())
                {
                    lExiste = true;
                }
                    
                lReader.Close();
                lTransaction.Commit();
                lConn.Close();
            }
            catch{
                if (lTransaction != null)
                    lTransaction.Rollback();
                if (lConn != null)
                    lConn.Close();
            }

            if (!lExiste)
                throw new Exception("Verifique su usuario y contraseña.");
        }

        public List<Usuario> RecuperarTodos()
        {
            NpgsqlTransaction lTransaction = null;
            NpgsqlConnection lConn = null;
            var lUsuarios = new List<Usuario>();

            try{
                lConn = new NpgsqlConnection(ConnectionString);                               
                lConn.Open();
                lTransaction = lConn.BeginTransaction(IsolationLevel.ReadCommitted);                    
                string lSql = "select username,id from users";
                NpgsqlCommand lCmd = new NpgsqlCommand(lSql, lConn);
                NpgsqlDataReader lReader = lCmd.ExecuteReader();

                Usuario lUsuario;
                while (lReader.Read())
                {
                    lUsuario = new Usuario();
                    lUsuario.Username = lReader.GetString(0);
                    lUsuario.Id = lReader.GetInt32(1);
                    lUsuarios.Add(lUsuario);
                }
                    
                lReader.Close();
                lTransaction.Commit();
                lConn.Close();
            }
            catch{
                if (lTransaction != null)
                    lTransaction.Rollback();
                if (lConn != null)
                    lConn.Close();
                throw;
            }

            return lUsuarios;
        }

        

        public void Guardar(Usuario AUsuario){
            NpgsqlTransaction lTransaction = null;
            NpgsqlConnection lConn = null;
            NpgsqlCommand lCmd = null;
            var lEsNuevo = (AUsuario.Id == 0);

            try{
                lConn = new NpgsqlConnection(ConnectionString);
                lConn.Open();
                lTransaction = lConn.BeginTransaction(IsolationLevel.ReadCommitted);

                string lSql = "";
                if (lEsNuevo){
                    lSql = "insert into users (username, password, enabled) " +
                        "values (@username, @password, @enabled) returning id";                
                    lCmd = new NpgsqlCommand(lSql, lConn);
                    lCmd.Parameters.AddWithValue("@username", AUsuario.Username);
                    lCmd.Parameters.AddWithValue("@password", AUsuario.Password);
                    lCmd.Parameters.AddWithValue("@enabled", AUsuario.Enabled);

                    NpgsqlDataReader lReader = lCmd.ExecuteReader();

                    var lId = 0;
                    while (lReader.Read())
                    {
                        lId = lReader.GetInt32(0);
                    }
                    lReader.Close();
                    AUsuario.Id = lId;

                    // guardar los roles.                
                    foreach (var lRole in AUsuario.Roles){
                        lSql = "insert into user_roles (id_user, role) " +
                            "values (@id_user, @role)";
                        lCmd = new NpgsqlCommand(lSql, lConn);
                        lCmd.Parameters.AddWithValue("@id_user", AUsuario.Id);
                        lCmd.Parameters.AddWithValue("@role", lRole.Role);
                        lCmd.ExecuteNonQuery();
                    }
                }
                else{
                    lSql = "update users set username = @username, password " +
                        "= @password, enabled = @enabled where id = @id";                
                    lCmd = new NpgsqlCommand(lSql, lConn);
                    lCmd.Parameters.AddWithValue("@username", AUsuario.Username);
                    lCmd.Parameters.AddWithValue("@password", AUsuario.Password);
                    lCmd.Parameters.AddWithValue("@enabled", AUsuario.Enabled);
                    lCmd.Parameters.AddWithValue("@id", AUsuario.Id);

                    lCmd.ExecuteNonQuery();

                    // actualizar los roles.

                    lSql = "delete from user_roles where id_user = @id_user";
                    lCmd = new NpgsqlCommand(lSql, lConn);
                    lCmd.Parameters.AddWithValue("@id_user", AUsuario.Id);
                    lCmd.ExecuteNonQuery();

                    foreach (var lRole in AUsuario.Roles){
                        lSql = "insert into user_roles (id_user, role) " +
                            "values (@id_user, @role)";
                        lCmd = new NpgsqlCommand(lSql, lConn);
                        lCmd.Parameters.AddWithValue("@id_user", AUsuario.Id);
                        lCmd.Parameters.AddWithValue("@role", lRole.Role);
                        lCmd.ExecuteNonQuery();
                    }
                }
                
                lTransaction.Commit();
                lConn.Close();
            }
            catch{
                if (lTransaction != null)
                    lTransaction.Rollback();
                if (lConn != null)
                    lConn.Close();
                throw;
            }
        }

        public void Eliminar(int AUsuarioId){   
            NpgsqlTransaction lTransaction = null;
            NpgsqlConnection lConn = null;

            try{
                lConn = new NpgsqlConnection(ConnectionString);
                lConn.Open();
                lTransaction = lConn.BeginTransaction(IsolationLevel.ReadCommitted);
                string lSql = "delete from user_roles where id_user = @id_user";
                NpgsqlCommand lCmd = new NpgsqlCommand(lSql, lConn);
                lCmd.Parameters.AddWithValue("@id_user", AUsuarioId);
                lCmd.ExecuteNonQuery();

                lSql = "delete from users where id = @id_user";
                lCmd = new NpgsqlCommand(lSql, lConn);
                lCmd.Parameters.AddWithValue("@id_user", AUsuarioId);
                lCmd.ExecuteNonQuery();
    
                lTransaction.Commit();
                lConn.Close();
            }
            catch{
                if (lTransaction != null)
                    lTransaction.Rollback();
                if (lConn != null)
                    lConn.Close();
                throw;
            }
        }

        public Usuario Recuperar(int AId){
            NpgsqlTransaction lTransaction = null;
            NpgsqlConnection lConn = null;
            var lUsuario = new Usuario();

            try{
                lConn = new NpgsqlConnection(ConnectionString);                               
                lConn.Open();
                lTransaction = lConn.BeginTransaction(IsolationLevel.ReadCommitted);                    
                string lSql = "select id, username, password, enabled from users where id = @id";
                NpgsqlCommand lCmd = new NpgsqlCommand(lSql, lConn);
                lCmd.Parameters.AddWithValue("@id", AId);
                NpgsqlDataReader lReader = lCmd.ExecuteReader();

                while (lReader.Read())
                {
                    lUsuario.Id = lReader.GetInt32(0);
                    lUsuario.Username = lReader.GetString(1);
                    lUsuario.Password = lReader.GetString(2);                    
                    lUsuario.Enabled = lReader.GetBoolean(3);
                }
                    
                lReader.Close();

                lSql = "select role from user_roles where id_user = @id";
                lCmd = new NpgsqlCommand(lSql, lConn);
                lCmd.Parameters.AddWithValue("@id", AId);
                lReader = lCmd.ExecuteReader();

                UsuarioRol lRole = null;
                while (lReader.Read())
                {
                    lRole = new UsuarioRol();
                    lRole.Role = lReader.GetString(0);  
                    lUsuario.Roles.Add(lRole);
                }
                    
                lReader.Close();

                lTransaction.Commit();
                lConn.Close();
            }
            catch{
                if (lTransaction != null)
                    lTransaction.Rollback();
                if (lConn != null)
                    lConn.Close();
                throw;
            }

            return lUsuario;
        }
    }
}