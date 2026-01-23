using PseudoFramework.SharedUtils;
using Servidor.DAOs;
using Servidor.DTOs;
using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.Rotas
{
    public class ExemploRota: IRota
    {
        public const string RECURSO = "exemplos";

        private static readonly ExemploDao _dao = new ExemploDao();

        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetExemplo(id);

                            return GetExemplos();
                        }
                    case "POST":
                        return PostExemplo(json);

                    case "PUT":
                        return PutExemplo(id, json);

                    case "DELETE":
                        return DeleteExemplo(id);

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de exemplo não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<ExemploSaidaDto>> GetExemplos()
        {
            var listaExemplosDto = _dao
                .Listar()
                .Select(exemploModel => new ExemploSaidaDto
                {
                    Id = exemploModel.Id,
                    Caracteristica1 = exemploModel.Caracteristica1,
                    Caracteristica2 = exemploModel.Caracteristica2,
                    Caracteristica3 = exemploModel.Caracteristica3,
                    Caracteristica4 = exemploModel.Caracteristica4
                });

            if (!listaExemplosDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaExemplosDto, "Lista de exemplos está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaExemplosDto);
        }

        private static RespostaWrapper<ExemploSaidaDto> GetExemplo(string id)
        {
            var exemploModelUnico = _dao
                .Listar()
                .FirstOrDefault(exemploModel => exemploModel.Id == Convert.ToInt32(id));

            if (exemploModelUnico == null)
                return RespostaWrapper.EnveloparInsucesso<ExemploSaidaDto>(null, "Exemplo não encontrado.");

            var exemploDtoConsultado = new ExemploSaidaDto
            {
                Id = exemploModelUnico.Id,
                Caracteristica1 = exemploModelUnico.Caracteristica1,
                Caracteristica2 = exemploModelUnico.Caracteristica2,
                Caracteristica3 = exemploModelUnico.Caracteristica3,
                Caracteristica4 = exemploModelUnico.Caracteristica4
            };

            return RespostaWrapper.EnveloparSucesso(exemploDtoConsultado);
        }

        private static RespostaWrapper<ExemploSaidaDto> PostExemplo(string json)
        {
            var listaExemplosModel = _dao.Listar();

            var exemploDtoRecebido = ConversorJson.Desserializar<ExemploEntradaDto>(json);

            var proximoId = listaExemplosModel.Any()
                ? listaExemplosModel.Max(exemploModel => exemploModel.Id) + 1
                : 1;

            var exemploModelIncluir = new ExemploModel
            {
                Id = proximoId,
                Caracteristica1 = exemploDtoRecebido.Caracteristica1,
                Caracteristica2 = exemploDtoRecebido.Caracteristica2,
                Caracteristica3 = exemploDtoRecebido.Caracteristica3,
                Caracteristica4 = exemploDtoRecebido.Caracteristica4
            };

            var exemploModelIncluido = _dao.Incluir(exemploModelIncluir);

            var exemploDtoIncluido = new ExemploSaidaDto
            {
                Id = exemploModelIncluido.Id,
                Caracteristica1 = exemploModelIncluido.Caracteristica1,
                Caracteristica2 = exemploModelIncluido.Caracteristica2,
                Caracteristica3 = exemploModelIncluido.Caracteristica3,
                Caracteristica4 = exemploModelIncluido.Caracteristica4
            };

            return RespostaWrapper.EnveloparSucesso(exemploDtoIncluido);
        }

        private static RespostaWrapper PutExemplo(string id, string json)
        {
            var exemploDtoRecebido = ConversorJson.Desserializar<ExemploEntradaDto>(json);

            var exemploModelAlterar = new ExemploModel
            {
                Caracteristica1 = exemploDtoRecebido.Caracteristica1,
                Caracteristica2 = exemploDtoRecebido.Caracteristica2,
                Caracteristica3 = exemploDtoRecebido.Caracteristica3,
                Caracteristica4 = exemploDtoRecebido.Caracteristica4
            };

            var resultado = _dao.Alterar(exemploModel => exemploModel.Id == Convert.ToInt32(id), exemploModelAlterar);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteExemplo(string id)
        {
            var listaExemplosModel = _dao.Listar();

            var idNumerico = Convert.ToInt32(id);

            if (!listaExemplosModel.Any(exemploModel => exemploModel.Id == idNumerico))
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrado um exemplo com o id informado.");

            var resultado = _dao.Remover(exemploModel => exemploModel.Id == Convert.ToInt32(id));

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}
