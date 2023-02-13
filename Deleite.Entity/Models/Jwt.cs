using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Deleite.Entity.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static dynamic validarToken(CaimsIdentity identity)
        {
            try
            {
                if(identity.Claims.Count() == 0) 
                {
                    return new
                    {
                        success = false,
                        message = "Verificar si estas enviando un token valido",
                        result = ""
                    };
                } 

                var id = identity.Claims.FirstOrDefault(x => x.Type == "IdUsuario").Value;

                Usuario usuario = Usuario.DB().FirstOrDefault(x => x.IdUsuario = id);

                return new
                {
                    success = true,
                    message = "exito",
                    result = usuario
                };

            }catch(Exception ex) 
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }

    }
}
