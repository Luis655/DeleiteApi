using Deleite.Dal.DBContext;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Deleite.Dal.Implementacion
{
    public class LoginToken2 : ILoginToken2
    {
        private readonly DeleitebdContext _dbcontext;
        public LoginToken2(DeleitebdContext dbcontext)
        {
            _dbcontext = dbcontext;
            
        }
        public async Task<Usuario> GetLoginData(int id)
        {
            var data = await _dbcontext.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            return data;
        }
    }
}