using PseudoFramework.SharedUtils;
using Servidor.DAOs;
using Servidor.DTOs;
using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.Rotas
{
    public class CategoriaRota : IRota
    {
        public const string RECURSO = "categorias";

        private static readonly CategoriaDao _dao = new CategoriaDao();

        public RespostaWrapper InterceptarRequisicao(string verboHttp, Uri caminho, string id, string json)
        {
            try
            {
                switch (verboHttp)
                {
                    case "GET":
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                                return GetCategoria(id);

                            return GetCategorias();
                        }
                    case "POST":
                        return PostCategoria(json);

                    case "PUT":
                        return PutCategoria(id, json);

                    case "DELETE":
                        return DeleteCategoria(id);

                    default:
                        return RespostaWrapper.EnveloparInsucesso("Operação de categoria não suportada.");
                }
            }
            catch (Exception exception)
            {
                return RespostaWrapper.EnveloparErro(exception);
            }
        }

        private static RespostaWrapper<IEnumerable<CategoriaDto>> GetCategorias()
        {
            var listaCategoriasDto = _dao
               .Listar()
               .Select(categoriaModel => new CategoriaDto
               {
                   Id = categoriaModel.Id,
                   Descricao = categoriaModel.Descricao,
                   UsuarioUltimaAlteracao = categoriaModel.UsuarioUltimaAlteracao,
                   DataHoraUltimaAlteracao = categoriaModel.DataHoraUltimaAlteracao
               });

            if (!listaCategoriasDto.Any())
                return RespostaWrapper.EnveloparInsucesso(listaCategoriasDto, "Lista de categorias está vazia.");

            return RespostaWrapper.EnveloparSucesso(listaCategoriasDto);
        }

        private static RespostaWrapper<CategoriaDto> GetCategoria(string id)
        {
            var categoriaModelUnica = _dao
                .Listar()
                .FirstOrDefault(categoriaModel => categoriaModel.Id == Convert.ToInt32(id));

            if (categoriaModelUnica == null)
                return RespostaWrapper.EnveloparInsucesso<CategoriaDto>(null, "Categoria não encontrada.");

            var categoriaDtoConsultada = new CategoriaDto
            {
                Id = categoriaModelUnica.Id,
                Descricao = categoriaModelUnica.Descricao,
                UsuarioUltimaAlteracao = categoriaModelUnica.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = categoriaModelUnica.DataHoraUltimaAlteracao
            };

            return RespostaWrapper.EnveloparSucesso(categoriaDtoConsultada);
        }

        private static RespostaWrapper<CategoriaDto> PostCategoria(string json)
        {
            var listaCategoriasModel = _dao.Listar();

            var categoriaDtoRecebida = ConversorJson.Desserializar<CategoriaDto>(json);

            if (listaCategoriasModel.Any(categoriaModel => categoriaModel.Id == categoriaDtoRecebida.Id))
                return RespostaWrapper.EnveloparInsucesso<CategoriaDto>(null, "Categoria já existe.");

            var categoriaModelIncluir = new CategoriaModel
            {
                Id = categoriaDtoRecebida.Id,
                Descricao = categoriaDtoRecebida.Descricao
            };

            var categoriaModelIncluida = _dao.Incluir(categoriaModelIncluir);

            var categoriaDtoIncluida = new CategoriaDto
            {
                Id = categoriaModelIncluida.Id,
                Descricao = categoriaModelIncluida.Descricao,
                UsuarioUltimaAlteracao = categoriaModelIncluida.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = categoriaModelIncluida.DataHoraUltimaAlteracao
            };

            return RespostaWrapper.EnveloparSucesso(categoriaDtoIncluida);
        }

        private static RespostaWrapper PutCategoria(string id, string json)
        {
            var categoriaDtoRecebida = ConversorJson.Desserializar<CategoriaDto>(json);

            var categoriaModelAlterar = new CategoriaModel
            {
                Descricao = categoriaDtoRecebida.Descricao,
                UsuarioUltimaAlteracao = categoriaDtoRecebida.UsuarioUltimaAlteracao,
                DataHoraUltimaAlteracao = DateTime.Now
            };

            var resultado = _dao.Alterar(categoriaModel => categoriaModel.Id == Convert.ToInt32(id), categoriaModelAlterar);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }

        private static RespostaWrapper DeleteCategoria(string id)
        {
            var listaCategoriasModel = _dao.Listar();

            var codigo = Convert.ToInt32(id);

            if (!listaCategoriasModel.Any(categoriaModel => categoriaModel.Id == codigo))
                return RespostaWrapper.EnveloparInsucesso("Não foi encontrada uma categoria com o código informado.");

            var resultado = _dao.Remover(categoriaModel => categoriaModel.Id == codigo);

            if (!resultado)
                return RespostaWrapper.EnveloparInsucesso();

            return RespostaWrapper.EnveloparSucesso();
        }
    }
}
