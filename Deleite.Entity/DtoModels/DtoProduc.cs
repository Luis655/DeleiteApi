using System;
using System.Collections.Generic;

namespace Deleite.Entity.DtoModels;

public partial class DtoProduc
{
    public int? IdProducto { get; set; }
    public int? IdCategoria { get; set; }
    public bool? IdConfirmacionT { get; set; }
    public int? IdTematica { get; set; }
    public string? NombreP { get; set; }
    public string? DescripcionP { get; set; }
    public string? Precio { get; set; }
    public string? ImagenPrincipal { get; set; }
    public bool? Popular { get; set; }
    public string? Ingredienteselect { get; set; }
    public bool? Saludable { get; set; }
    public string? NombreTematica { get; set; }
    public string? NombreCategoria { get; set; }


}
