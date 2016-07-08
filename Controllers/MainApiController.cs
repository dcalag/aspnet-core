using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DcaLag.AspNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DcaLag.AspNet.Controllers {
    [Authorize]
    public class MainApiController: Controller {

        private IUsuarioDao FUsuarioDao;
        private SqlSettings FSqlSettings;
         
        public MainApiController(IUsuarioDao AUsuarioDao, IOptions<SqlSettings> ASettings){
            FUsuarioDao = AUsuarioDao;
            FSqlSettings = ASettings.Value;
            FUsuarioDao.ConnectionString = FSqlSettings.ConnectionString;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseModel> Login([FromBody] LoginData AData)
        {
            var lResponse = new BaseModel();

            try{
                // Verificar en la bd de seguridad...
                FUsuarioDao.ValidarUsuario(AData.Username, AData.Password);
                var lLista = FUsuarioDao.RecuperarRoles(AData.Username);

                GenericIdentity lGenIdentity = new GenericIdentity(AData.Username, 
                    "CustomAuthDcaLag_AspNet");
                ClaimsPrincipal principal = new ClaimsPrincipal(lGenIdentity);
                var identity = principal.Identity as ClaimsIdentity;
                foreach (var lRole in lLista){
                    identity.AddClaim(new Claim(ClaimTypes.Role, lRole));                
                }
                
                await HttpContext.Authentication.SignInAsync("CustomSchemeDcaLag_AspNet", principal);                
            }
            catch (Exception ex){
                lResponse.Error = ex.Message;
            }
            return lResponse;
        }
    }
}