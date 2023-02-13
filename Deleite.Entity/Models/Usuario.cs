using System;
using System.Collections.Generic;

namespace Deleite.Entity.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; }

    public string Correo { get; set; }
  

    public string Contraseña { get; set; }

   // public string rol { get; set; }

    public string Token { get; set; }

    public DateTime? FechaToken { get; set; }

    public static List<Usuario> DB()
    {
        var List = new List<Usuario>();
        {
            new Usuario
            {
                IdUsuario = 1,
                Nombre = "Angel",
                Correo = "prueba@gmail.com",
                Contraseña = "1234",
                Token = string.Empty,
                FechaToken = null,
               // rol = "Administrador"
            }; 
            new Usuario
            {
                IdUsuario = 1,
                Nombre = "Luis",
                Correo = "prueba@gmail.com",
                Contraseña = "1234",
                Token = string.Empty,
                FechaToken = null,

            };

        }
        return List;
       
         
    }
}
