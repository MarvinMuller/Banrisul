using Cliente.Telas;
using PseudoFramework.ClienteUtils;
using System;

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
                ExibirMenuPrincipal();

                var opcaoTela = ObterOpcaoTela();

                if (opcaoTela == "S")
                    break;

                Console.WriteLine();

                ExecutarTela(opcaoTela, cliente);
            }

            cliente.Encerrar();

            Console.ReadKey();
        }

        private static void ExibirMenuPrincipal()
        {
            Console.WriteLine("1 - Exemplos");
            Console.WriteLine("2 - Idiomas");
            Console.WriteLine("3 - Categorias");
            Console.WriteLine("S - Sair");
        }

        private static string ObterOpcaoTela()
        {
            Console.Write("Selecione a Tela: ");

            return Console.ReadLine().Trim().ToUpper();
        }

        private static void ExecutarTela(string opcaoTela, ClienteHttp cliente)
        {
            Tela tela;

            switch (opcaoTela)
            {
                case "1":
                    {
                        tela = new TelaExemplos(cliente);
                        break;
                    }
                case "2":
                    {
                        tela = new TelaIdiomas(cliente);
                        break;
                    }
                case "3":
                    {
                        tela = new TelaCategorias(cliente);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Tela não existe.");
                        Console.WriteLine();

                        return;
                    }
            }

            while (true)
            {
                tela.ExibirOpcoes();

                var opcao = tela.ObterOpcao();

                Console.WriteLine();

                if (opcao == "S")
                    break;

                tela.ExecutarOpcao(opcao);
            }
        }
    }
}
