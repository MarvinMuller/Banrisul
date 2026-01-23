using PseudoFramework.ServidorUtils;
using Servidor.Models;
using System;
using System.IO;

namespace Servidor.DAOs
{
    public class ExemploDao: ObjetoPersistenciaJson<ExemploModel>
    {
        private const string ARQUIVO = "exemplos.json";

        private static string RaizProjeto = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "data"); // Alterar conforme conveniência

        public ExemploDao() : base(Path.Combine(RaizProjeto, ARQUIVO)) { }
    }
}
