using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Deleite.Dal.DBContext;
using Microsoft.EntityFrameworkCore;
using Deleite.Dal.Implementacion;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using FluentValidation;
using Deleite.Bll.Validators;
using Deleite.Entity.DtoModels;
/*using Deleite.Bll.Implementacion;
using Deleite.Bll.Interfaces;*/

namespace Deleite.IOC;
public static class dependencia
{
    public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration){
        services.AddDbContext<DeleitebdContext>(options=>{
            options.UseSqlServer(Configuration.GetConnectionString("coneccionBD"));
        });
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient(typeof(ILoginToken), typeof(LoginToken));
        services.AddTransient(typeof(ILoginToken2), typeof(LoginToken2));
        services.AddTransient<IValidator<DtoUserLogin>, CreateLoginRequestValidators>();


        //services.AddScoped<IVentaRepository, VentaRepository>();

    }
}
