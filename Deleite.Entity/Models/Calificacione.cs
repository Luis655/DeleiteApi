using System;
using System.Collections.Generic;

namespace Deleite.Entity.Models;

public partial class Calificacione
{
    public int? Idcalificacion { get; set; }

    public int? Idproducto { get; set; }

    public int? Estrellas { get; set; }

    public virtual Producto? IdproductoNavigation { get; set; }
}
