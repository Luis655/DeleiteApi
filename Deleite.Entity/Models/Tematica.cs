using System;
using System.Collections.Generic;

namespace Deleite.Entity.Models;

public partial class Tematica
{
    public int? IdTematica { get; set; }

    public string? NombreT { get; set; }

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
