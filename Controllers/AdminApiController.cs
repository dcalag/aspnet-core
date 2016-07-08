using System;
using System.Collections.Generic;
using DcaLag.AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DcaLag.AspNet.Controllers{
    [Authorize (Roles ="ROLE_ADMIN")]
    public class AdminApiController{

        private IUsuarioDao FUsuarioDao;
        private SqlSettings FSqlSettings;
         
        public AdminApiController(IUsuarioDao AUsuarioDao,
            IOptions<SqlSettings> ASettings){
            FUsuarioDao = AUsuarioDao;
            FSqlSettings = ASettings.Value;
            FUsuarioDao.ConnectionString = FSqlSettings.ConnectionString;
        }

        [HttpGet]
        [Route("AdminApi/Usuarios")]
        public List<Usuario> UsuariosGet() {
            return FUsuarioDao.RecuperarTodos();
        }

        [HttpGet]
        [Route("AdminApi/Usuario/{userId}")]
        public Usuario UsuarioGet(string userId){
            var lModel = new Usuario();
            try{
                lModel = FUsuarioDao.Recuperar(Int32.Parse(userId));
            }
            catch (Exception ex){
                lModel.Error = ex.Message;
            }
            return lModel;
        }

        [HttpDelete]
        [Route("AdminApi/Usuario/{userId}")]
        public BaseModel UsuarioDelete(string userId){
            var lModel = new BaseModel();
            try{
                FUsuarioDao.Eliminar(int.Parse(userId));
            }
            catch (Exception ex){
                lModel.Error = ex.Message;
            }
            return lModel;
        }

        [HttpPost]
        [Route("AdminApi/Usuario")]
        public BaseModel UsuarioPost([FromBody] Usuario AUsuario){            
            var lResponse = new BaseModel();

            try{
                FUsuarioDao.Guardar(AUsuario);
            }
            catch (Exception ex){
                lResponse.Error = ex.Message;
            }

            return lResponse;
        }

        [HttpPut]
        [Route("AdminApi/Usuario/{userId}")]
        public BaseModel UsuarioPut([FromBody] Usuario AUsuario, string userId){            
            var lResponse = new BaseModel();

            AUsuario.Id = Int32.Parse(userId);

            try{
                FUsuarioDao.Guardar(AUsuario);
            }
            catch (Exception ex){
                lResponse.Error = ex.Message;
            }

            return lResponse;
        }
    }
}
