using Cliente.DTOs;
using PseudoFramework.ClienteUtils;
using System;
using System.Collections.Generic;

namespace Cliente.Telas
{
    public class TelaExemplos: Tela
    {
        private const string ROTA = "exemplos";

        public TelaExemplos(ClienteHttp cliente) : base(cliente) { }

        public override void ExibirOpcoes()
        {
            Console.WriteLine("1 - Listar Exemplos (GET)");
            Console.WriteLine("2 - Consultar Exemplo (GET/id)");
            Console.WriteLine("3 - Incluir Exemplo (POST)");
            Console.WriteLine("4 - Alterar Exemplo (PUT/id)");
            Console.WriteLine("5 - Remover Exemplo (DELETE/id)");
            Console.WriteLine("S - Voltar ao Menu Principal");
        }

        public override void ExecutarOpcao(string opcao)
        {
            switch (opcao)
            {
                case "1":
                    {
                        ListarExemplos();
                        break;
                    }
                case "2":
                    {
                        ConsultarExemplo();
                        break;
                    }
                case "3":
                    {
                        IncluirExemplo();
                        break;
                    }
                case "4":
                    {
                        AlterarExemplo();
                        break;
                    }
                case "5":
                    {
                        RemoverExemplo();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Opção inválida.");
                        Console.WriteLine();

                        break;
                    }
            }
        }

        private void ListarExemplos()
        {
            var wrapperResposta = ExecutorHttp.ExecutarGet<IEnumerable<ExemploEntradaDto>>(_cliente, ROTA);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            foreach (var exemploEntradaDto in wrapperResposta.Dados)
            {
                Console.WriteLine($"Exemplo {exemploEntradaDto.Id}");
                Console.WriteLine(exemploEntradaDto.Caracteristica1);
                Console.WriteLine(exemploEntradaDto.Caracteristica2);
                Console.WriteLine(exemploEntradaDto.Caracteristica3);
                Console.WriteLine(exemploEntradaDto.Caracteristica4);

                Console.WriteLine();
            }
        }

        private void ConsultarExemplo()
        {
            Console.Write("Informe o id do exemplo: ");
            string id = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{id}";

            var wrapperResposta = ExecutorHttp.ExecutarGet<ExemploEntradaDto>(_cliente, rotaComposta);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            var exemploEntradaDto = wrapperResposta.Dados;

            Console.WriteLine($"Exemplo {exemploEntradaDto.Id}");
            Console.WriteLine(exemploEntradaDto.Caracteristica1);
            Console.WriteLine(exemploEntradaDto.Caracteristica2);
            Console.WriteLine(exemploEntradaDto.Caracteristica3);
            Console.WriteLine(exemploEntradaDto.Caracteristica4);

            Console.WriteLine();
        }

        private void IncluirExemplo()
        {
            var exemploSaidaDto = new ExemploSaidaDto
            {
                Caracteristica1 = "Teste Inclusão",
                Caracteristica2 = 100,
                Caracteristica3 = true,
                Caracteristica4 = 1200.20
            };

            var wrapperResposta = ExecutorHttp.ExecutarPost<ExemploSaidaDto, ExemploEntradaDto>(_cliente, ROTA, exemploSaidaDto);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            var exemploEntradaDto = wrapperResposta.Dados;

            if (exemploEntradaDto != null)
            {
                Console.WriteLine($"Exemplo {exemploEntradaDto.Id}");
                Console.WriteLine(exemploEntradaDto.Caracteristica1);
                Console.WriteLine(exemploEntradaDto.Caracteristica2);
                Console.WriteLine(exemploEntradaDto.Caracteristica3);
                Console.WriteLine(exemploEntradaDto.Caracteristica4);

                Console.WriteLine();
            }
        }

        private void AlterarExemplo()
        {
            Console.Write("Informe o id do exemplo: ");
            string id = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{id}";

            var exemploSaidaDto = new ExemploSaidaDto
            {
                Caracteristica1 = "Teste Alteração",
                Caracteristica2 = 200,
                Caracteristica3 = false,
                Caracteristica4 = 2400.40
            };

            var wrapperResposta = ExecutorHttp.ExecutarPut(_cliente, rotaComposta, exemploSaidaDto);

            Console.WriteLine(wrapperResposta.Mensagem);
            Console.WriteLine();
        }

        private void RemoverExemplo()
        {
            Console.Write("Informe o id do exemplo: ");
            string id = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{id}";

            var wrapperResposta = ExecutorHttp.ExecutarDelete(_cliente, rotaComposta);

            Console.WriteLine(wrapperResposta.Mensagem);
            Console.WriteLine();
        }
    }
}
