namespace Deleite.Entity.DtoModels
{
    public class DtoImagenesCategorias
    {
        public int? IdCategoria {get; set;}
        public string Base64 {get; set;}
        public string Nombre {get; set;}
        public string Tipo {get; set;}
        public string Imagen { get; set; }
        public string? NombreCategoria { get; set; }

    }
}