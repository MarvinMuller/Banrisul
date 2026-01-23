using PseudoFramework.SharedUtils;

namespace PseudoFramework.ClienteUtils
{
    public class ExecutorHttp
    {
        public static RespostaWrapper<T> ExecutarGet<T>(ClienteHttp cliente, string rota)
        {
            var jsonResposta = cliente.EnviarGet($"{ConectorHttp.ObterCaminho()}{rota}");

            var wrapperResposta = ConversorJson.Desserializar<RespostaWrapper<T>>(jsonResposta);

            return wrapperResposta;
        }

        public static RespostaWrapper<TOut> ExecutarPost<TIn, TOut>(ClienteHttp cliente, string rota, TIn objetoRequisicao)
        {
            var jsonResposta = cliente.EnviarPost($"{ConectorHttp.ObterCaminho()}/{rota}", objetoRequisicao);

            var wrapperResposta = ConversorJson.Desserializar<RespostaWrapper<TOut>>(jsonResposta);

            return wrapperResposta;
        }

        public static RespostaWrapper ExecutarPut<T>(ClienteHttp cliente, string rota, T objetoRequisicao)
        {
            var jsonResposta = cliente.EnviarPut($"{ConectorHttp.ObterCaminho()}/{rota}", objetoRequisicao);

            var wrapperResposta = ConversorJson.Desserializar<RespostaWrapper>(jsonResposta);

            return wrapperResposta;
        }

        public static RespostaWrapper ExecutarDelete(ClienteHttp cliente, string rota)
        {
            var jsonResposta = cliente.EnviarDelete($"{ConectorHttp.ObterCaminho()}/{rota}");

            var wrapperResposta = ConversorJson.Desserializar<RespostaWrapper>(jsonResposta);

            return wrapperResposta;
        }
    }
}
