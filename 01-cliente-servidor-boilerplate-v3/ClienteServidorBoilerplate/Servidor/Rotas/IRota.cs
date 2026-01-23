using PseudoFramework.SharedUtils;
using System;

namespace Servidor.Rotas
{
    public interface IRota
    {
        RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json);
    }
}
