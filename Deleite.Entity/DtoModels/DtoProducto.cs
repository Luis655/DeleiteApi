namespace Deleite.Entity.DtoModels
{
public class DtoProduco
{
    public int? IdProducto { get; set; }
    public int? IdCategoria { get; set; }
    public bool? IdConfirmacionT { get; set; }
    public int? IdTematica { get; set; }
    public string? NombreP { get; set; }
    public string? DescripcionP { get; set; }
    public string? Precio { get; set; }
    public byte[]? ImagenPrincipalchar { get; set; }
    public bool? Popular { get; set; }
    public string? Ingredienteselect { get; set; }
    public bool? Saludable { get; set; }

}

}