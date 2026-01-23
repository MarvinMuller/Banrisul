using System;

namespace Cliente.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
