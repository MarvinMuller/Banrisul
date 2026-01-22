using System;
using System.IO;
using System.Net;
using System.Text;
using PseudoFramework.SharedUtils;

namespace PseudoFramework.ClienteUtils
{
    public class ClienteHttp
    {
        public static string IDENTIFICADOR = "CLIENTE";

        public string EnviarGet(string caminho)
        {
            return EnviarRequisicaoSemCorpo("GET", caminho);
        }

        public string EnviarPost<T>(string caminho, T objetoRequisicaoJson)
        {
            var json = ConversorJson.Serializar(objetoRequisicaoJson);

            return EnviarRequisicaoComCorpo("POST", caminho, json);
        }

        public string EnviarPut<T>(string caminho, T objetoRequisicaoJson)
        {
            string json = ConversorJson.Serializar(objetoRequisicaoJson);

            return EnviarRequisicaoComCorpo("PUT", caminho, json);
        }

        public string EnviarDelete(string caminho)
        {
            return EnviarRequisicaoSemCorpo("DELETE", caminho);
        }

        public void Encerrar()
        {
            Console.WriteLine($"\nEncerrado.\n\n");
        }

        private string EnviarRequisicaoSemCorpo(string verboHttp, string caminho)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json; charset=utf-8");
                client.Headers.Add("Accept", "application/json");

                Console.WriteLine($" [i] Enviando requisição {verboHttp} para {caminho}... ");

                try
                {
                    byte[] corpoRespostaBytes = verboHttp.ToUpper().Equals("GET")
                        ? client.DownloadData(caminho)
                        : client.UploadData(caminho, verboHttp, new byte[0]);

                    var corpoResposta = Encoding.UTF8.GetString(corpoRespostaBytes);

                    ImprimirCorpoResposta(corpoResposta);

                    return corpoResposta;
                }
                catch (WebException excecao)
                {
                    return TratarExcecao(excecao);
                }
            }
        }

        private string EnviarRequisicaoComCorpo(string verboHttp, string caminho, string corpoRequisicaoJson)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json; charset=utf-8");
                client.Headers.Add("Accept", "application/json");

                Console.WriteLine($" [i] Enviando requisição {verboHttp} para {caminho} com o corpo JSON <{corpoRequisicaoJson}>...");

                try
                {
                    var corpoRequisicaoBytes = Encoding.UTF8.GetBytes(corpoRequisicaoJson);

                    var corpoRespostaBytes = client.UploadData(caminho, verboHttp, corpoRequisicaoBytes);

                    var corpoResposta = Encoding.UTF8.GetString(corpoRespostaBytes);

                    ImprimirCorpoResposta(corpoResposta);

                    return corpoResposta;
                }
                catch (WebException excecao)
                {
                    return TratarExcecao(excecao);
                }
            }
        }

        private string TratarExcecao(WebException excecao)
        {
            var mensagem = $" [x] ERRO {excecao.Message}";

            if (excecao.Response != null)
            {
                using (var respostaStream = excecao.Response.GetResponseStream())
                using (var reader = new StreamReader(respostaStream))
                {
                    var corpoResposta = reader.ReadToEnd();

                    mensagem = $"{mensagem}\n          Recebido: {corpoResposta}";
                }
            }

            Console.WriteLine($"{mensagem}\n");

            return mensagem;
        }

        private void ImprimirCorpoResposta(string corpoResposta)
        {
            Console.WriteLine($"     Recebido: {corpoResposta}\n");
        }
    }
}
