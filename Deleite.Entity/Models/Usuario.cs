using System;
using System.Collections.Generic;

namespace Deleite.Entity.Models;

public partial class Usuario
{
    public int? IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Contraseña { get; set; }

    public string? Token { get; set; }

    public DateTime? FechaToken { get; set; }
}
