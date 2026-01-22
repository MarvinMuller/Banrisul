using PseudoFramework.SharedUtils;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PseudoFramework.ServidorUtils
{
    public class ServidorHttp
    {
        public static string IDENTIFICADOR = "SERVIDOR";

        public Func<string, string, string, object> ProcessarHook { get; set; }

        private bool _executando;
        private string _caminhoBase;

        private HttpListener _listener;

        public ServidorHttp(string caminhoBase)
        {
            _executando = false;

            _caminhoBase = caminhoBase;

            _listener = new HttpListener();

            _listener.Prefixes.Add(_caminhoBase);
        }

        public void Iniciar()
        {
            _listener.Start();

            _executando = true;

            Console.WriteLine($"Iniciado e aguardando requisições em {_caminhoBase}\n");

            Task.Run(
                () =>
                {
                    while (_executando)
                    {
                        try
                        {
                            var contexto = _listener.GetContext();

                            Task.Run(() => ProcessarRequisicao(contexto));
                        }
                        catch (HttpListenerException)
                        {
                            if (_executando) throw;
                        }
                        catch (Exception excecao)
                        {
                            Console.WriteLine($" [x] ERRO {excecao.Message}");
                        }
                    }
                }
            );
        }

        public void Encerrar()
        {
            Console.Write($"Encerrando... ");

            _executando = false;

            _listener.Stop();

            Console.Write($"Encerrado.\n\n");
        }

        private void ProcessarRequisicao(HttpListenerContext contexto)
        {
            var requisicaoObjeto = contexto.Request;

            var verboHttp = requisicaoObjeto.HttpMethod;

            var caminho = requisicaoObjeto.Url.AbsoluteUri;

            var momento = DateTime.Now;

            string corpoRequisicaoJson = null;
            if (requisicaoObjeto.HasEntityBody)
            {
                var requisicaoBuffer = new byte[requisicaoObjeto.ContentLength64];

                using (var requisicaoStream = requisicaoObjeto.InputStream)
                {
                    var corpoRequisicaoBytes = requisicaoStream.Read(requisicaoBuffer, 0, requisicaoBuffer.Length);

                    corpoRequisicaoJson = Encoding.UTF8.GetString(requisicaoBuffer, 0, corpoRequisicaoBytes);
                }
            }

            var corpoRequisicaoExpressao = string.IsNullOrWhiteSpace(corpoRequisicaoJson)
                ? "SEM CORPO" :
                $"com o corpo JSON <{corpoRequisicaoJson}>";

            Console.Write($" [i] Requisição {verboHttp} recebida em {caminho} às {momento} {corpoRequisicaoExpressao}... ");

            var respostaObjeto = contexto.Response;

            respostaObjeto.StatusCode = 200;
            respostaObjeto.ContentType = "application/json; charset=utf-8";

            object dadosResposta = ProcessarHook != null
                ? ProcessarHook(verboHttp, caminho, corpoRequisicaoJson)
                : new
                    {
                        origem = IDENTIFICADOR,
                        verboHttp = verboHttp,
                        caminho = caminho,
                        momento = momento,
                        respostaString = $"Resposta de {IDENTIFICADOR} para a requisição {verboHttp} em {caminho} processada às {momento} {corpoRequisicaoExpressao}: OK"
                    };

            var corpoRespostaJson = ConversorJson.Serializar(dadosResposta);

            var respostaBuffer = Encoding.UTF8.GetBytes(corpoRespostaJson);

            respostaObjeto.ContentLength64 = respostaBuffer.Length;

            using (var respostaStream = respostaObjeto.OutputStream)
            {
                respostaStream.Write(respostaBuffer, 0, respostaBuffer.Length);
            }

            Console.Write("Resposta JSON enviada\n\n");

            respostaObjeto.Close();
        }
    }
}
