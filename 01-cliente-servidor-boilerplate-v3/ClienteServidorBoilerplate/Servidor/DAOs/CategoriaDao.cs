using PseudoFramework.ServidorUtils;
using Servidor.Models;
using System;
using System.IO;

namespace Servidor.DAOs
{
    public class CategoriaDao : ObjetoPersistenciaJson<CategoriaModel>
    {
        private const string ARQUIVO = "categorias.json";

        private static string RaizProjeto = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "data");

        public CategoriaDao() : base(Path.Combine(RaizProjeto, ARQUIVO)) { }
    }
}
