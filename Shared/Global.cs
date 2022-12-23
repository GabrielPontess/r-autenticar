using RAutenticar.Models;

namespace RAutenticar.Shared
{
    public class Global
    {
        private static Global _global;
        public DadosValidacao DadosValidacao { get; set; }

        
        public static Global Instance
        {
            get
            {
                if(_global == null)
                {
                    _global = new Global();
                    _global.DadosValidacao = new DadosValidacao();
                    _global.DadosValidacao.Atendente = new Atendente();
                }

                return _global;
            }
        }
    }
}
