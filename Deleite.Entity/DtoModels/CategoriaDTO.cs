using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deleite.Entity.DtoModels
{
    public class CategoriaDTO
    {
        public int? IdProducto { get; set; }
        public string? NombreP { get; set; }
        public string? DescripcionP { get; set; }
        public string? Precio { get; set; }
        public bool? Saludable { get; set; }
    }
}
