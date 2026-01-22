using PseudoFramework.ServidorUtils;
using PseudoFramework.SharedUtils;
using System;
using System.Linq;

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
                (verbo, caminho, json) => InterceptarRequisicao(verbo, caminho, json);

            servidor.Iniciar();

            Console.ReadKey();

            servidor.Encerrar();

            Console.ReadKey();
        }

        private static Saudacao InterceptarRequisicao(string verboHttp, string caminho, string json)
        {
            switch (verboHttp)
            {
                case "PUT":
                    return InterceptarPut(json);
                case "POST":
                    return InterceptarPost(json);
                case "GET":
                    return InterceptarGet();
                case "DELETE":
                    return InterceptarDelete();

                default:
                    return null;
            }
        }

        private static Saudacao InterceptarPut(string json)
        {
            var saudacaoCliente = ConversorJson.Desserializar<Saudacao>(json);

            string cumprimentos = string.Concat(Enumerable.Repeat(saudacaoCliente.Cumprimento + " ", saudacaoCliente.NumeroCumprimentos));

            string saudacaoClienteExpressao = $"{cumprimentos}{saudacaoCliente.PronomeTratamento} {saudacaoCliente.Nome}!";

            Console.WriteLine($"\n\n{saudacaoClienteExpressao} \n");

            var saudacaoServidor = new Saudacao()
            {
                Cumprimento = "Olá",
                NumeroCumprimentos = 1,
                PronomeTratamento = "Sr.",
                Nome = "Cliente",
            };

            return saudacaoServidor;
        }

        private static Saudacao InterceptarPost(string json)
        {
            var saudacaoCliente = ConversorJson.Desserializar<Saudacao>(json);

            string montagemString = $"O(A) cliente, quer ser chamado de {saudacaoCliente.PronomeTratamento} {saudacaoCliente.Nome} cumprimentou {saudacaoCliente.NumeroCumprimentos} vezes, dizendo a palavra {saudacaoCliente.Cumprimento}";

            Console.WriteLine($"\n\n{montagemString} \n");

            Saudacao respostaServer = new Saudacao()
            {
                Cumprimento = "Eae",
                NumeroCumprimentos = 2,
                PronomeTratamento = "meu patrão",
                Nome = saudacaoCliente.Nome,
            };

            return respostaServer;
        }

        private static Saudacao InterceptarGet()
        {
            Saudacao resposta = new Saudacao()
            {
                Cumprimento = "Olá,",
                NumeroCumprimentos = 1,
                PronomeTratamento = "o comando GET foi interceptado caro",
                Nome = "Cliente",
            };

            return resposta;
        }

        private static Saudacao InterceptarDelete()
        {
            Saudacao resposta = new Saudacao()
            {
                Cumprimento = "Ei,",
                NumeroCumprimentos = 1,
                PronomeTratamento = "não há nada para deletar aqui, calma lá",
                Nome = "amigão",
            };

            return resposta;
        }
    }
}