using System.Web.Mvc;

namespace Sistema.Stoque.v1.UI.Controllers
{// Para Fazer Testar para verificar se exite erros
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string matricula)
        {
            RedirectToAction("CadastrarHorimetroAdm", "Horimetro",matricula);
            return View();
        }
    }
}