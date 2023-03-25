using System.Xml;
using System.ComponentModel;
using Deleite.Dal.DBContext;
using Deleite.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Deleite.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Deleite.Entity.DtoModels;

namespace Deleite.Dal.Implementacion
{
    public class LoginToken : ILoginToken
    {
        private readonly DeleitebdContext _dbcontext;
        public LoginToken(DeleitebdContext dbcontext)
        {
            _dbcontext = dbcontext;

        }
        public async Task<Usuario> GetLogin(Usuario u)
        {
            var data = await _dbcontext.Usuarios
            .FirstOrDefaultAsync(x => x.Nombre == u.Nombre && x.Contraseña == u.Contraseña);
            return data;
        }

        public async Task<DtoUserLogin> getuserDto(DtoUserLogin u)
        {
            var data = await _dbcontext.Usuarios
            .FirstOrDefaultAsync(x => x.Correo == u.Correo && x.Contraseña == u.Contraseña);
            if (data != null) {
                var dto = new DtoUserLogin
                {
                    Correo = data.Correo == null ? "No se encontro el correo" : data.Correo,
                    Contraseña = data.Contraseña
                };
                return dto;
            } else {
                throw new Exception("no se encontraron datos");
            }

        }
        public async Task<Usuario> GetLoginData(string id)
        {
            var data = await _dbcontext.Usuarios.FirstOrDefaultAsync(x => x.Correo == id);
            return data;
        }

        public async Task InvalidateToken(string token)
        {
            // ... código para invalidar el token ...
        }


    }
}