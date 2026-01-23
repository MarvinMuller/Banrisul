using PseudoFramework.ServidorUtils;
using PseudoFramework.SharedUtils;
using Servidor.Rotas;
using System;

namespace Servidor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var servidor = new ServidorHttp(ConectorHttp.ObterCaminho());

            Console.WriteLine(":::::::::::::::::");
            Console.WriteLine($":::: {ServidorHttp.IDENTIFICADOR} :::");
            Console.WriteLine(":::::::::::::::::\n");

            Console.WriteLine("Pressione ENTER para encerrar...\n");

            servidor.ProcessarHook =
                (verbo, caminho, json) => RotearRequisicao(verbo, caminho, json);

            servidor.Iniciar();

            Console.ReadKey();

            servidor.Encerrar();

            Console.ReadKey();
        }

        private static RespostaWrapper RotearRequisicao(string verboHttp, string caminho, string json)
        {
            IRota rota;

            var uriCaminho = new Uri(caminho);

            var partesRota = uriCaminho.AbsolutePath
                .TrimStart('/')
                .ToLower()
                .Split('/');

            var recurso = partesRota.Length > 0
                ? partesRota[0]
                : null;

            switch (recurso)
            {
                case ExemploRota.RECURSO:
                    {
                        rota = new ExemploRota();

                        break;
                    }

                case IdiomaRota.RECURSO:
                    {
                        rota = new IdiomaRota();

                        break;
                    }

                case CategoriaRota.RECURSO:
                    {
                        rota = new CategoriaRota();

                        break;
                    }

                default:
                    return RespostaWrapper.EnveloparInsucesso("Rota não suportada.");
            }

            var id = partesRota.Length > 1
                ? partesRota[1]
                : null;

            return rota.InterceptarRequisicao(verboHttp, uriCaminho, id, json);
        }
    }
}
