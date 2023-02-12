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
        //services.AddScoped<IVentaRepository, VentaRepository>();
    }
}
