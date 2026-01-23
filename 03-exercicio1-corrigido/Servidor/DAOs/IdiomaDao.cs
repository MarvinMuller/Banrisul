using PseudoFramework.ServidorUtils;
using Servidor.Models;
using System;
using System.IO;

namespace Servidor.DAOs
{
    public class IdiomaDao: ObjetoPersistenciaJson<IdiomaModel>
    {
        // Nome do arquivo que será criado/mantido
        private const string ARQUIVO = "idiomas.json";

        // Pasta onde o arquivo ficará armazenado — Toda essa lógica aqui serve pra navegar até a raíz da solution
        private static string RaizProjeto = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "data"); // Alterar conforme conveniencia

        // Construtor repassando para a base (para o framework) o endereço da pasta e o nome do arquivo
        public IdiomaDao() : base(Path.Combine(RaizProjeto, ARQUIVO)) { }
    }
}
