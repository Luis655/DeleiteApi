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
        public async Task<Usuario> GetLoginData(string id)
        {
            var idd = Int64.Parse(id);
            var data = await _dbcontext.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idd);
            return data;
        }

    }
}