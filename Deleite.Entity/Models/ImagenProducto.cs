using System;
using System.Collections.Generic;

namespace Deleite.Entity.Models;

public partial class ImagenProducto
{
    public int? IdimgProducto { get; set; }

    public string? NombreFoto { get; set; }

    public int? IdProducto { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
