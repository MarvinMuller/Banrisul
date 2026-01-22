using System.Web.Script.Serialization;

namespace PseudoFramework.SharedUtils
{
    public static class ConversorJson
    {
        private static readonly JavaScriptSerializer _serializr = new JavaScriptSerializer();

        public static string Serializar(object objeto)
        {
            return _serializr.Serialize(objeto);
        }

        public static T Desserializar<T>(string json)
        {
            return _serializr.Deserialize<T>(json);
        }

        public static object DesserializarParaObjeto(string json)
        {
            return _serializr.DeserializeObject(json);
        }
    }
}
