using PseudoFramework.SharedUtils;
using Servidor.DAOs;
using Servidor.DTOs;
using Servidor.Models;
using Servidor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.Rotas
{
    public class IdiomaRota: IRota
    {
        public const string RECURSO = "idiomas";

        private static readonly IdiomaDao _dao = new IdiomaDao();

        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetIdioma(id); // US5

                            return GetIdiomas(); // US4
                        }
                    case "POST":
                        return PostIdioma(json); // US1

                    case "PUT":
                        return PutIdioma(id, json); // US3

                    case "DELETE":
                        return DeleteIdioma(id); // US2

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de idioma não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<IdiomaDto>> GetIdiomas()
        {
            var listaIdiomasDto = _dao
                .Listar()
                .Select(idiomaModel => new IdiomaDto
                {
                    Id = idiomaModel.Id,
                    CodigoIsoCombinado = Feconid.CodigoToIso(idiomaModel.Id),
                    Descricao = idiomaModel.Descricao
                });

            if (!listaIdiomasDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaIdiomasDto, "Lista de idiomas está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaIdiomasDto);
        }

        private static RespostaWrapper<IdiomaDto> GetIdioma(string id)
        {
            var idiomaModelUnico = _dao
                .Listar()
                .FirstOrDefault(idiomaModel => idiomaModel.Id == Convert.ToInt32(id));

            if (idiomaModelUnico == null)
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null, "Idioma não encontrado.");

            var idiomaDtoConsultado = new IdiomaDto
            {
                Id = idiomaModelUnico.Id,
                CodigoIsoCombinado = Feconid.CodigoToIso(idiomaModelUnico.Id),
                Descricao = idiomaModelUnico.Descricao,
                UsuarioUltimaAlteracao = idiomaModelUnico.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = idiomaModelUnico.DataHoraUltimaAlteracao
            };

            return RespostaWrapper.EnveloparSucesso(idiomaDtoConsultado);
        }

        private static RespostaWrapper<IdiomaDto> PostIdioma(string json)
        {
            var listaIdiomasModel = _dao.Listar();

            var idiomaDtoRecebido = ConversorJson.Desserializar<IdiomaDto>(json);

            var codigo = Feconid.IsoToCodigo(idiomaDtoRecebido.CodigoIsoCombinado);

            if (listaIdiomasModel.Any(idiomaModel => idiomaModel.Id == codigo))
                return RespostaWrapper.EnveloparInsucesso<IdiomaDto>(null, "Idioma já existe.");

            var idiomaModelIncluir = new IdiomaModel
            {
                Id = codigo,
                Descricao = idiomaDtoRecebido.Descricao
            };

            var idiomaModelIncluido = _dao.Incluir(idiomaModelIncluir);

            var idiomaDtoIncluido = new IdiomaDto
            {
                Id = idiomaModelIncluido.Id,
                CodigoIsoCombinado = Feconid.CodigoToIso(idiomaModelIncluido.Id),
                Descricao = idiomaModelIncluido.Descricao,
                UsuarioUltimaAlteracao = idiomaModelIncluido.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = idiomaModelIncluido.DataHoraUltimaAlteracao
            };

            return RespostaWrapper.EnveloparSucesso(idiomaDtoIncluido);
        }

        private static RespostaWrapper PutIdioma(string id, string json)
        {
            var idiomaDtoRecebido = ConversorJson.Desserializar<IdiomaDto>(json);

            var idiomaModelAlterar = new IdiomaModel
            {
                Descricao = idiomaDtoRecebido.Descricao,
                UsuarioUltimaAlteracao = idiomaDtoRecebido.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = DateTime.Now
            };

            var resultado = _dao.Alterar(idiomaModel => idiomaModel.Id == Convert.ToInt32(id), idiomaModelAlterar);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteIdioma(string id)
        {
            var listaIdiomasModel = _dao.Listar();

            var codigo = Convert.ToInt32(id);

            if (!listaIdiomasModel.Any(idiomaModel => idiomaModel.Id == codigo))
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrado um idioma com o código informado.");

            var resultado = _dao.Remover(idiomaModel => idiomaModel.Id == codigo);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}