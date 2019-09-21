using System.Web.Mvc;
using Sistema.Stoque.v1.Aplicacao;

namespace Sistema.Stoque.v1.UI.Controllers
{
    public class UserController : Controller
    {
        public readonly UsuarioAplicacao UsuarioAPP;

        public UserController()
        {
            UsuarioAPP = AplicacaoConstrutores.UsuarioAplicacaoConstrutor();
        }
        // GET: User
        public ActionResult Criar()
        {
            return View();
        }
    }
}