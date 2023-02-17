using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Deleite.Entity.Models;
using System.Security.Claims;
using Deleite.Dal.DBContext;
using Deleite.Dal.Interfaces;

namespace Deleite.Bll.Jwt
{
    public class Jwt
    {
        private readonly ILoginToken _dbcontext;
        public Jwt(ILoginToken dbcontect)
        {
            _dbcontext = dbcontect;
        }
        public TokenResult validarToken(ClaimsIdentity identity)
        {
           
                if(identity.Claims.Count() == 0) 
                {
                   
                    return new TokenResult
                    {
                        success = false,
                        message = "Verificar si estas enviando un token valido",
                        result = ""
                    };
                } 

                var id = identity.Claims.FirstOrDefault(x => x.Type == "IdUsuario").Value;

                //var idd = Int32.Parse(id);
                
                var usuario = _dbcontext.GetLoginData(id);
                //Usuario usuario = Usuario.DB().FirstOrDefault(x => x.IdUsuario == Int32.Parse(id));

                return new TokenResult
                {
                    success = true,
                    message = "exito",
                    result =  "usuario.Result.Nombre"
                };

         
        }


    }

}
