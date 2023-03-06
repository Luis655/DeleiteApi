using System;
using System.Collections.Generic;

namespace Deleite.Entity.Models;

public partial class Categoria
{
    public int? IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
