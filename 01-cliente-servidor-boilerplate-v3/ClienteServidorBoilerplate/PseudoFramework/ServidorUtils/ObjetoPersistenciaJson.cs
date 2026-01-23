using PseudoFramework.SharedUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PseudoFramework.ServidorUtils
{
    public class ObjetoPersistenciaJson<T> where T : class
    {
        private readonly string _arquivo;

        private List<T> _itens;

        public ObjetoPersistenciaJson(string arquivo)
        {
            _arquivo = arquivo;

            if (!File.Exists(_arquivo))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_arquivo));

                File.Create(_arquivo).Close();
            }

            _itens = ConversorJson.Desserializar<List<T>>(File.ReadAllText(_arquivo)) ?? new List<T>();
        }

        public IEnumerable<T> Listar()
        {
            return _itens;
        }

        public T Incluir(T item)
        {
            _itens.Add(item);

            Salvar();

            return item;
        }

        public bool Alterar(Predicate<T> condicao, T item)
        {
            var indice = _itens.FindIndex(condicao);

            if (indice < 0)
                return false;

            var itemOriginal = _itens[indice];

            AtualizarCampos(itemOriginal, item);

            Salvar();

            return true;
        }

        public bool Remover(Predicate<T> condicao)
        {
            var indice = _itens.FindIndex(condicao);

            if (indice < 0)
                return false;

            _itens.RemoveAt(indice);

            Salvar();

            return true;
        }

        private void AtualizarCampos(T destino, T origem)
        {
            // Seleciona por reflection todas as propriedades de um objeto T (mimetizando campos de uma tabela) que possam sofrer escrita — exceto a propriedade cujo nome seja exatamente "Id"
            var camposAtualizaveis = typeof(T)
                .GetProperties()
                .Where(prop => prop.CanWrite && !string.Equals(prop.Name, "Id"));

            foreach (var propriedade in camposAtualizaveis)
            {
                var valor = propriedade.GetValue(origem);

                propriedade.SetValue(destino, valor);
            }
        }

        private void Salvar()
        {
            File.WriteAllText(_arquivo, ConversorJson.Serializar(_itens));
        }
    }
}
