namespace Deleite.Entity.Models
{
    public class resultado
    {
        public int? idimgProducto {get; set;}
        public string? Base64Origihal {get; set;}
        public string? Base64 {get; set;}
        public string? Nombre {get; set;}
        public string? Tipo {get; set;}
        public string? NombreP { get; set; }
        public string? DescripcionP { get; set; }
        public string? Precio { get; set; }
        public string? categoria { get; set; }
        public string? tematica { get; set; }
        public bool? Popular { get; set; }
        public string Ingredienteselect { get; set; }
        public bool? Saludable { get; set; }
        
    }
}