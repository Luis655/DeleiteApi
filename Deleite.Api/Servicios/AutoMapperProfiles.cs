using AutoMapper;
using Deleite.Entity.DtoModels;
using Deleite.Entity.Models;

namespace Deleite.Api.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Categoria, CategoriaDTO>();
        }
    }
}
