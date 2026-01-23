using PseudoFramework.ClienteUtils;
using System;

namespace Cliente.Telas
{
    public abstract class Tela
    {
        protected ClienteHttp _cliente;

        public Tela(ClienteHttp cliente)
        {
            _cliente = cliente;
        }

        public abstract void ExibirOpcoes();

        public virtual string ObterOpcao()
        {
            Console.Write("Selecione a ação: ");

            return Console.ReadLine().Trim().ToUpper();
        }

        public abstract void ExecutarOpcao(string opcao);
    }
}
