using Cliente.DTOs;
using PseudoFramework.ClienteUtils;
using System;
using System.Collections.Generic;

namespace Cliente.Telas
{
    public class TelaCategorias: Tela
    {
        private const string ROTA = "categorias";

        public TelaCategorias(ClienteHttp cliente) : base(cliente) { }

        public override void ExibirOpcoes()
        {
            Console.WriteLine("1 - Listar Categorias (GET)");
            Console.WriteLine("2 - Consultar Categoria (GET/id)");
            Console.WriteLine("3 - Incluir Categoria (POST)");
            Console.WriteLine("4 - Alterar Categoria (PUT/id)");
            Console.WriteLine("5 - Remover Categoria (DELETE/id)");
            Console.WriteLine("S - Voltar ao Menu Principal");
        }

        public override void ExecutarOpcao(string opcao)
        {
            switch (opcao)
            {
                case "1":
                    {
                        ListarCategorias();
                        break;
                    }
                case "2":
                    {
                        ConsultarCategoria();
                        break;
                    }
                case "3":
                    {
                        IncluirCategoria();
                        break;
                    }
                case "4":
                    {
                        AlterarCategoria();
                        break;
                    }
                case "5":
                    {
                        RemoverCategoria();
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

        private void ListarCategorias()
        {
            var wrapperResposta = ExecutorHttp.ExecutarGet<IEnumerable<CategoriaDto>>(_cliente, ROTA);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            foreach (var categoriaDto in wrapperResposta.Dados)
            {
                Console.WriteLine($"Categoria {categoriaDto.Id}");
                Console.WriteLine(categoriaDto.Descricao);

                Console.WriteLine();
            }
        }

        private void ConsultarCategoria()
        {
            Console.Write("Informe o código da categoria: ");
            string codigo = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{codigo}";

            var wrapperResposta = ExecutorHttp.ExecutarGet<CategoriaDto>(_cliente, rotaComposta);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            var categoriaDto = wrapperResposta.Dados;

            Console.WriteLine($"Categoria {categoriaDto.Id}");
            Console.WriteLine(categoriaDto.Descricao);

            if (!string.IsNullOrWhiteSpace(categoriaDto.UsuarioUltimaAlteracao))
                Console.WriteLine($"Última alteração por {categoriaDto.UsuarioUltimaAlteracao} em {categoriaDto.DataHoraUltimaAlteracao.ToLocalTime()}");

            Console.WriteLine();
        }

        private void IncluirCategoria()
        {
            Console.Write("Informe o código da categoria: ");
            string codigo = Console.ReadLine().Trim();

            Console.Write("Informe a descrição da categoria: ");
            string descricao = Console.ReadLine().Trim();

            Console.WriteLine();

            var novaCategoriaDto = new CategoriaDto
            {
                Id = Convert.ToInt32(codigo),
                Descricao = descricao
            };

            var wrapperResposta = ExecutorHttp.ExecutarPost<CategoriaDto, CategoriaDto>(_cliente, ROTA, novaCategoriaDto);

            if (!wrapperResposta.Sucesso)
            {
                Console.WriteLine(wrapperResposta.Mensagem);
                Console.WriteLine();

                return;
            }

            var categoriaIncluidaDto = wrapperResposta.Dados;

            if (categoriaIncluidaDto != null)
            {
                Console.WriteLine($"Categoria {categoriaIncluidaDto.Id}");
                Console.WriteLine(categoriaIncluidaDto.Descricao);

                Console.WriteLine();
            }
        }

        private void AlterarCategoria()
        {
            Console.Write("Informe o código da categoria: ");
            string codigo = Console.ReadLine().Trim();

            Console.Write("Informe a nova descrição da categoria: ");
            string descricao = Console.ReadLine().Trim();

            Console.Write("Informe o seu nome de usuário: "); // Hipotético — para fins didáticos
            string usuario = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{codigo}";

            var categoriaAlterarDto = new CategoriaDto
            {
                Descricao = descricao,
                UsuarioUltimaAlteracao = usuario
            };

            var wrapperResposta = ExecutorHttp.ExecutarPut(_cliente, rotaComposta, categoriaAlterarDto);

            Console.WriteLine(wrapperResposta.Mensagem);
            Console.WriteLine();
        }

        private void RemoverCategoria()
        {
            Console.Write("Informe o código da categoria: ");
            string codigo = Console.ReadLine().Trim();

            Console.WriteLine();

            var rotaComposta = $"{ROTA}/{codigo}";

            var wrapperResposta = ExecutorHttp.ExecutarDelete(_cliente, rotaComposta);

            Console.WriteLine(wrapperResposta.Mensagem);
            Console.WriteLine();
        }
    }
}
