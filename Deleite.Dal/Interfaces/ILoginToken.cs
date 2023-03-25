using System.Linq;
using Deleite.Entity.DtoModels;
using Deleite.Entity.Models;
namespace Deleite.Dal.Interfaces
{
    public interface ILoginToken
    {
         Task<Usuario> GetLogin(Usuario usuario);
         Task<DtoUserLogin> getuserDto(DtoUserLogin user);

        Task InvalidateToken(string token);

        Task<Usuario> GetLoginData(string id);
    }
}