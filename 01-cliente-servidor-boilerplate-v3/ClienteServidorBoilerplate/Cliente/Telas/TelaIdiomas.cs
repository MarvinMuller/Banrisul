using Cliente.DTOs;
using PseudoFramework.ClienteUtils;
using System;
using System.Collections.Generic;

namespace Cliente.Telas
{
    public class TelaIdiomas: Tela
    {
        private const string ROTA = "idiomas";

        public TelaIdiomas(ClienteHttp cliente) : base(cliente) { }

        public override void ExibirOpcoes()
        {
            Console.WriteLine("1 - Listar Idiomas (GET)");
            Console.WriteLine("2 - Consultar Idioma (GET/id)");
            Console.WriteLine("3 - Incluir Idioma (POST)");
            Console.WriteLine("4 - Alterar Idioma (PUT/id)");
            Console.WriteLine("5 - Remover Idioma (DELETE/id)");
            Console.WriteLine("S - Voltar ao Menu Principal");
        }

        public override void ExecutarOpcao(string opcao)
        {
            switch (opcao)
            {
                case "1":
                    {
                        ListarIdiomas();
                        break;
                    }
                case "2":
                    {
                        ConsultarIdioma();
                        break;
                    }
                case "3":
                    {
                        IncluirIdioma();
                        break;
                    }
                case "4":
                    {
                        AlterarIdioma();
                        break;
                    }
                case "5":
                    {
                        RemoverIdioma();
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

        private void ListarIdiomas()
        {
            var wrapperResposta = ExecutorHttp.ExecutarGet<IEnumerable<IdiomaDto>>(_cliente, ROTA);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            foreach (var idiomaDto in wrapperResposta.Dados)
            {
                Console.WriteLine($"Idioma {idiomaDto.Id} / {idiomaDto.CodigoIsoCombinado}");
                Console.WriteLine(idiomaDto.Descricao);

                Console.WriteLine();
            }
        }

        private void ConsultarIdioma()
        {
            Console.Write("Informe o código numérico do idioma: ");
            string codigo = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{codigo}";

            var wrapperResposta = ExecutorHttp.ExecutarGet<IdiomaDto>(_cliente, rotaComposta);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            var idiomaDto = wrapperResposta.Dados;

            Console.WriteLine($"Idioma {idiomaDto.Id} / {idiomaDto.CodigoIsoCombinado}");
            Console.WriteLine(idiomaDto.Descricao);

            if (!string.IsNullOrWhiteSpace(idiomaDto.UsuarioUltimaAlteracao))
                Console.WriteLine($"Última alteração por {idiomaDto.UsuarioUltimaAlteracao} em {idiomaDto.DataHoraUltimaAlteracao.ToLocalTime()}");

            Console.WriteLine();
        }

        private void IncluirIdioma()
        {
            Console.Write("Informe o código ISO combinado do idioma (ex: pt-BR, en-US): ");
            string codigoIso = Console.ReadLine().Trim();

            Console.Write("Informe a descrição do idioma: ");
            string descricao = Console.ReadLine().Trim();

            Console.WriteLine();

            var novoIdiomaDto = new IdiomaDto
            {
                CodigoIsoCombinado = codigoIso,
                Descricao = descricao
            };

            var wrapperResposta = ExecutorHttp.ExecutarPost<IdiomaDto, IdiomaDto>(_cliente, ROTA, novoIdiomaDto);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            var idiomaIncluidoDto = wrapperResposta.Dados;

            if (idiomaIncluidoDto != null)
            {
                Console.WriteLine($"Idioma {idiomaIncluidoDto.Id} / {idiomaIncluidoDto.CodigoIsoCombinado}");
                Console.WriteLine(idiomaIncluidoDto.Descricao);

                Console.WriteLine();
            }
        }

        private void AlterarIdioma()
        {
            Console.Write("Informe código numérico do idioma: ");
            string codigo = Console.ReadLine().Trim();

            Console.Write("Informe a nova descrição do idioma: ");
            string descricao = Console.ReadLine().Trim();

            Console.Write("Informe o seu nome de usuário: "); // Hipotético — para fins didáticos
            string usuario = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{codigo}";

            var idiomaAlterarDto = new IdiomaDto
            {
                Descricao = descricao,
                UsuarioUltimaAlteracao = usuario
            };

            var wrapperResposta = ExecutorHttp.ExecutarPut(_cliente, rotaComposta, idiomaAlterarDto);

            Console.WriteLine(wrapperResposta.Mensagem);
            Console.WriteLine();
        }

        private void RemoverIdioma()
        {
            Console.Write("Informe o código numérico do idioma: ");
            string codigo = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{codigo}";

            var wrapperResposta = ExecutorHttp.ExecutarDelete(_cliente, rotaComposta);

            Console.WriteLine(wrapperResposta.Mensagem);
            Console.WriteLine();
        }
    }
}
