using Sistema.Stoque.v1.Repositorio;

namespace Sistema.Stoque.v1.Aplicacao
{
   public class AplicacaoConstrutores
    {
       

        public static ProdutosAplicacao ProdutosAplicacaoConstrutor() {
            return new ProdutosAplicacao(new ProdutosRepositorioADO());
        }

        public static SaidasAplicacao SaidasAplicacaoConstrutor() {
            return new SaidasAplicacao(new SaidasRepositorioADO());
        }

        public static EntradaAplicacao EntradasAplicacaoConstrutor()
        {
            return new EntradaAplicacao(new EntradasRepositorioADO());
        }
        public static UsuarioAplicacao UsuarioAplicacaoConstrutor()
        {
            return new UsuarioAplicacao(new UsuarioRepositorioADO());
        }


    }
}
