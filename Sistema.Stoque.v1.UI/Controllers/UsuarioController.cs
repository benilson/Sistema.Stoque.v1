using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistema.Stoque.v1.Aplicacao;
using Sistema.Stoque.v1.Dominio;

namespace Sistema.Stoque.v1.UI.Controllers
{
    public class UsuarioController : Controller
    {
        public readonly UsuarioAplicacao UsuarioAPP;
        public static string idUsuario;

        public UsuarioController()
        {
            UsuarioAPP = AplicacaoConstrutores.UsuarioAplicacaoConstrutor();
        }

        public ActionResult LoginUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUsuario(Usuario usuario)
        {
            var usuarios = UsuarioAPP.ShowAll().ToList();
            foreach (var usuario1 in usuarios)
            {
                if (usuario.NomeUsuario == usuario1.NomeUsuario && usuario.SenhaUsuario == usuario1.SenhaUsuario)
                {
                    Session["usuario"] = usuario1;
                    if(usuario1.SenhaUsuario == "123")
                    {
                        idUsuario = usuario1.Id_Usuario.ToString(); 
                        return RedirectToAction("Editar");
                    }

                    idUsuario = usuario1.Id_Usuario.ToString();
                    return RedirectToAction("Index", "Home");
                }
            }
                    ViewBag.Validacoes = "O Senha ou Usuario Não Existe";

            return View(usuario);
        }

        public ActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Criar(Usuario usuario, HttpPostedFileBase file, string perfil)
        {
            var fotoNome = "";

            if (file != null)
            {
                fotoNome = Path.GetFileName(file.FileName);
                var caminho = Path.Combine(Server.MapPath("~/ImgProdutos"), fotoNome);
                file.SaveAs(caminho);
            } else {
                
            }


            if (ModelState.IsValid)
            {
                usuario.urlFoto = fotoNome;
                usuario.PerfilUsuario = perfil;
                UsuarioAPP.Salvar(usuario);
                return RedirectToAction("ListaDeUsuarios");
            }

            return View("Criar");
        }

        public ActionResult Logout()
        {
            Session["usuario"] = null;
            return RedirectToAction("LoginUsuario");
        }


        //Lista todos o Usuarioes na bd
        public ActionResult ListaDeUsuarios()
        {
            return View(UsuarioAPP.ShowAll());
        }
        //Edita Usuario na bd
        public ActionResult Editar(string id)
        {
            if (id == null)
            {
                var usuario = UsuarioAPP.ListarPorId(idUsuario);
                return View(usuario);

            }

            var Usuarioes = UsuarioAPP.ListarPorId(id);
            if (Usuarioes == null)
                return HttpNotFound();

            return View(Usuarioes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Usuario Usuario, string pass)
        {

            //Confirmação da senha
            var UsuarioLogado = Session["usuario"] as Usuario;

            if (UsuarioLogado.SenhaUsuario != pass)
            {
                ViewBag.Erro = "Senha invalida";
                return View(Usuario);

            }
            else
            {
                if (Usuario.SenhaUsuario != null)
                {
                    UsuarioAPP.Salvar(Usuario);
                    return RedirectToAction("Listarprodutos", "Produtos");
                }
                else {
                    return View(Usuario);
                }
            }
        }

        //Elimina Usuario da bd
        public ActionResult Eliminar(string id)
        {
            var Usuario = UsuarioAPP.ListarPorId(id);

            if (Usuario == null)
                return HttpNotFound();

            return View(Usuario);
        }


        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(string id)
        {
            var igreja = UsuarioAPP.ListarPorId(id);
            UsuarioAPP.Delete(igreja);

            return RedirectToAction("ListaDeUsuarios");
        }


    }
}