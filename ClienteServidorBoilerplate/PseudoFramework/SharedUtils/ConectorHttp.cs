namespace PseudoFramework.SharedUtils
{
    public static class ConectorHttp
    {
        private static string PROTOCOLO = "http";
        private static string DOMINIO = "localhost";
        private static int PORTA = 3090;

        public static string ObterCaminho()
        {
            return $"{PROTOCOLO}://{DOMINIO}:{PORTA}/";
        }
    }
}
