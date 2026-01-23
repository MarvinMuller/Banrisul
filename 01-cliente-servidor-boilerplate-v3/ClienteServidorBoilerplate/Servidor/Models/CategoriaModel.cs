using System;

namespace Servidor.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime DataHoraUltimaAlteracao { get; set; }
    }
}
