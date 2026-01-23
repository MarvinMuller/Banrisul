using System;

namespace PseudoFramework.SharedUtils
{
    public class RespostaWrapper
    {
        public bool Sucesso { get; set; }
        public bool Erro { get; set; } = false;
        public string Mensagem { get; set; }
        public DateTime Momento { get; set; }

        public static RespostaWrapper EnveloparSucesso()
        {
            return EnveloparSucesso("Operação realizada com sucesso.");
        }

        public static RespostaWrapper EnveloparSucesso(string mensagem)
        {
            return new RespostaWrapper()
            {
                Sucesso = true,
                Mensagem = mensagem,
                Momento = DateTime.Now
            };
        }

        public static RespostaWrapper EnveloparInsucesso()
        {
            return EnveloparInsucesso("Operação não foi realizada.");
        }

        public static RespostaWrapper EnveloparInsucesso(string mensagem)
        {
            return new RespostaWrapper()
            {
                Sucesso = false,
                Mensagem = mensagem,
                Momento = DateTime.Now
            };
        }

        public static RespostaWrapper EnveloparErro(Exception exception)
        {
            return EnveloparErro($"Operação falhou: {exception.Message}");
        }

        public static RespostaWrapper EnveloparErro(string mensagem)
        {
            return new RespostaWrapper()
            {
                Sucesso = false,
                Erro = true,
                Mensagem = mensagem,
                Momento = DateTime.Now,
            };
        }

        public static RespostaWrapper<T> EnveloparSucesso<T>(T dados)
        {
            return EnveloparSucesso(dados, "Operação realizada com sucesso.");
        }

        public static RespostaWrapper<T> EnveloparSucesso<T>(T dados, string mensagem)
        {
            return new RespostaWrapper<T>()
            {
                Sucesso = true,
                Mensagem = mensagem,
                Momento = DateTime.Now,
                Dados = dados
            };
        }

        public static RespostaWrapper<T> EnveloparInsucesso<T>(T dados)
        {
            return EnveloparInsucesso(dados, "Operação não foi realizada.");
        }

        public static RespostaWrapper<T> EnveloparInsucesso<T>(T dados, string mensagem)
        {
            return new RespostaWrapper<T>()
            {
                Sucesso = false,
                Mensagem = mensagem,
                Momento = DateTime.Now,
                Dados = dados
            };
        }

        public static RespostaWrapper<T> EnveloparErro<T>(T dados, Exception exception)
        {
            return EnveloparErro(dados, $"Operação falhou: {exception.Message}");
        }

        public static RespostaWrapper<T> EnveloparErro<T>(T dados, string mensagem)
        {
            return new RespostaWrapper<T>()
            {
                Sucesso = false,
                Erro = true,
                Mensagem = mensagem,
                Momento = DateTime.Now,
                Dados = dados
            };
        }
    }

    public class RespostaWrapper<T>: RespostaWrapper
    {
        public T Dados { get; set; }
    }
}
