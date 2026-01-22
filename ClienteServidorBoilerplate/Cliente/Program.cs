using PseudoFramework.ClienteUtils;
using PseudoFramework.SharedUtils;
using System;
using System.Linq;

namespace Cliente
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cliente = new ClienteHttp();

            Console.WriteLine("::::::::::::::::::");
            Console.WriteLine($":::: {ClienteHttp.IDENTIFICADOR} :::::");
            Console.WriteLine("::::::::::::::::::\n");

            while (true)
            {
                ExibirOpcoes();

                var opcao = ObterOpcao();

                if (opcao == "S")
                    break;

                Console.WriteLine();

                var saudacao = ExecutarOpcao<Saudacao>(cliente, opcao);

                if (saudacao != null)
                {
                    string cumprimentos = string.Concat(Enumerable.Repeat(saudacao.Cumprimento + " ", saudacao.NumeroCumprimentos));

                    Console.WriteLine($"{cumprimentos}{saudacao.PronomeTratamento} {saudacao.Nome}!");

                    Console.WriteLine();
                }
            }

            cliente.Encerrar();

            Console.ReadKey();
        }

        private static void ExibirOpcoes()
        {
            Console.WriteLine("1 - GET");
            Console.WriteLine("2 - POST");
            Console.WriteLine("3 - PUT");
            Console.WriteLine("4 - DELETE");
            Console.WriteLine("S - Sair");
        }

        private static string ObterOpcao()
        {
            Console.Write("Selecione a ação: ");

            return Console.ReadLine().Trim().ToUpper();
        }

        private static T ExecutarOpcao<T>(ClienteHttp cliente, string opcao)
        {
            var caminho = ConectorHttp.ObterCaminho();

            switch (opcao)
            {
                case "1":
                    return ExecutarGet<T>(cliente);

                case "2":
                    return ExecutarPost<Saudacao, T>(cliente, SolicitarSaudacao());

                case "3":
                    return ExecutarPut<Saudacao, T>(cliente, SolicitarSaudacao());

                case "4":
                    return ExecutarDelete<T>(cliente);

                default:
                    {
                        Console.WriteLine("Opção inválida.");

                        Console.WriteLine();

                        return default(T);
                    }
            }
        }

        private static T ExecutarGet<T>(ClienteHttp cliente)
        {
            var jsonResposta = cliente.EnviarGet(ConectorHttp.ObterCaminho());

            T objetoResposta = ConversorJson.Desserializar<T>(jsonResposta);

            return objetoResposta;
        }

        private static TOut ExecutarPost<TIn, TOut>(ClienteHttp cliente, TIn objetoRequisicao)
        {
            var jsonResposta = cliente.EnviarPost(ConectorHttp.ObterCaminho(), objetoRequisicao);

            TOut objetoResposta = ConversorJson.Desserializar<TOut>(jsonResposta);

            return objetoResposta;
        }

        private static TOut ExecutarPut<TIn, TOut>(ClienteHttp cliente, TIn objetoRequisicao)
        {
            var jsonResposta = cliente.EnviarPut(ConectorHttp.ObterCaminho(), objetoRequisicao);

            TOut objetoResposta = ConversorJson.Desserializar<TOut>(jsonResposta);

            return objetoResposta;
        }

        private static T ExecutarDelete<T>(ClienteHttp cliente)
        {
            var jsonResposta = cliente.EnviarDelete(ConectorHttp.ObterCaminho());

            T objetoResposta = ConversorJson.Desserializar<T>(jsonResposta);

            return objetoResposta;
        }

        private static Saudacao SolicitarSaudacao()
        {
            var saudacao = new Saudacao();

            Console.Write("Digita o pronome de tratamento (sr., Mr., Sra., Srta., etc.: ");

            saudacao.PronomeTratamento = Console.ReadLine().Trim();

            Console.Write("Digita o nome da pessoa: ");

            saudacao.Nome = Console.ReadLine().Trim();

            Console.Write("Digita a saudação: ");

            saudacao.Cumprimento = Console.ReadLine().Trim();

            // TODO (OPCIONAL): Implemente a captura de número de cumprimentos também via console
            saudacao.NumeroCumprimentos = 2;

            Console.WriteLine();

            return saudacao;
        }
    }
}