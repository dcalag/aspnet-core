using System;
using System.Collections.Generic;
using DcaLag.AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DcaLag.AspNet.Controllers{
    
    [Authorize (Roles ="ROLE_ADMIN")]
    public class AdminApiController{

        private IUsuarioDao FUsuarioDao;
        private SqlSettings FSqlSettings;
        private ILogger<AdminApiController> FLogger;
         
        public AdminApiController(IUsuarioDao AUsuarioDao, ILogger<AdminApiController> ALogger,
            IOptions<SqlSettings> ASettings){
            FUsuarioDao = AUsuarioDao;
            FLogger = ALogger;
            FSqlSettings = ASettings.Value;
            FUsuarioDao.ConnectionString = FSqlSettings.ConnectionString;
        }

        [HttpGet]
        [Route("AdminApi/Usuarios")]
        public List<Usuario> UsuariosGet() {
            // FLogger.LogError("error!");                      
            return FUsuarioDao.RecuperarTodos();
        }

        [HttpGet]
        [Route("AdminApi/Usuario/{userId}")]
        public Usuario UsuarioGet(string userId){
            var lModel =
                FUsuarioDao.Recuperar(Int32.Parse(userId));
            return lModel;
        }

        [HttpDelete]
        [Route("AdminApi/Usuario/{userId}")]
        public void UsuarioDelete(string userId){
            FUsuarioDao.Eliminar(int.Parse(userId));
        }

        [HttpPost]
        [Route("AdminApi/Usuario")]
        public void UsuarioPost([FromBody] Usuario AUsuario){ 
            FUsuarioDao.Guardar(AUsuario);            
        }

        [HttpPut]
        [Route("AdminApi/Usuario/{userId}")]
        public void UsuarioPut([FromBody] Usuario AUsuario, string userId){ 
            AUsuario.Id = Int32.Parse(userId);
            FUsuarioDao.Guardar(AUsuario);
        }
    }
}
