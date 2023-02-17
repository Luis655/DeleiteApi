using System.Data;
using Deleite.Entity.Models;

namespace Deleite.Dal.Interfaces
{
    public interface ILoginToken2
    {
         Task<Usuario> GetLoginData(int id);
    }
}