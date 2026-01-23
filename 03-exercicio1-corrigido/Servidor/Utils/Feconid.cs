using System;
using System.Text;

namespace Servidor.Utils
{
    /* 
    * Atenção
    * 
    * Esse utilitário foi implementado por IA com revisão superficial, e demanda 
    * refinamento do algoritmo e revisão das validações e questões de segurança para 
    * ser utilizado em qualquer outro ambiente que não seja o didático.
    * 
    */
    public static class Feconid
    {
        private const int B_27 = 27;

        public static int IsoToCodigo(string iso)
        {
            if (string.IsNullOrEmpty(iso))
                throw new ArgumentException("código combinado ISO não pode ser nulo ou vazio.");

            var parts = iso.Split('-');
            if (parts.Length != 2)
                throw new ArgumentException("Formato inválido do código combinado ISO. Deve ser 'll-PP' ou 'lll-PP'.");

            string idioma = parts[0].ToLower();
            string pais = parts[1].ToUpper();

            if (idioma.Length < 2 || idioma.Length > 3)
                throw new ArgumentException("Idioma deve ter 2 a 3 caracteres.");

            if (pais.Length != 2)
                throw new ArgumentException("País deve ter 2 caracteres.");

            int idiomaLength = idioma.Length;

            int codigo = idiomaLength;

            foreach (var caractere in idioma)
                codigo = codigo * B_27 + CaractereToNumero(caractere);

            foreach (var ch in pais)
                codigo = codigo * B_27 + CaractereToNumero(ch);

            return codigo;
        }

        public static string CodigoToIso(int codigo)
        {
            if (codigo <= 0)
                throw new ArgumentException("Código numérico inválido.");

            var iso = new StringBuilder();

            while (codigo > 0)
            {
                int remainder = codigo % B_27;

                iso.Insert(0, NumeroToCaractere(remainder));

                codigo = codigo / B_27;
            }

            if (iso.Length < 3)
                throw new Exception("Número inválido de caracteres para conversão.");

            int idiomaLength = CaractereToNumero(iso[0]);
            if (idiomaLength < 2 || idiomaLength > 3)
                throw new ArgumentException("Idioma deve ter 2 a 3 caracteres.");

            string idioma = iso.ToString(1, idiomaLength).ToLower();

            string pais = iso.ToString(1 + idiomaLength, iso.Length - 1 - idiomaLength).ToUpper();

            return $"{idioma}-{pais}";
        }

        private static int CaractereToNumero(char caractere)
        {
            caractere = char.ToUpper(caractere);

            if (caractere < 'A' || caractere > 'Z')
                throw new ArgumentException($"Caractere {caractere} inválido.");

            return caractere - 'A' + 1;
        }

        private static char NumeroToCaractere(int numero)
        {
            if (numero < 1 || numero > 26)
                throw new ArgumentException($"Número {numero} inválido.");

            return (char)('A' + numero - 1);
        }
    }
}
