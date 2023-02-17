using System.Linq;
using Deleite.Entity.Models;
namespace Deleite.Dal.Interfaces
{
    public interface ILoginToken
    {
         Task<Usuario> GetLogin(Usuario usuario);
        Task<Usuario> GetLoginData(string id);
    }
}