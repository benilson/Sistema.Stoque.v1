using System.IO;
using System.Web;
using System.Web.Mvc;
using Sistema.Stoque.v1.Aplicacao;
using Sistema.Stoque.v1.Dominio;

namespace Sistema.Stoque.v1.UI.Controllers
{
    public class ProdutoArmazemController : Controller
    {
        public readonly ProdutosAplicacao produtosAPP;

        public ProdutoArmazemController()
        {
            produtosAPP = AplicacaoConstrutores.ProdutosAplicacaoConstrutor();
        }
        // GET: produto
        public ActionResult CadastrarProdutos()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarProdutos(Produtos produtos, HttpPostedFileBase file)
        {

            var fotoNome = Path.GetFileName(file.FileName);
            var caminho = Path.Combine(Server.MapPath("~/ImgProdutos"), fotoNome);
            file.SaveAs(caminho);

            if (ModelState.IsValid)
            {
                produtos.Imagem = fotoNome;
                produtosAPP.Salvar(produtos);
                return RedirectToAction("Listarprodutos");
            }

            return View("Index");
        }

        public ActionResult ListarprodutosEpedidos()
        {//mostra os produtos dos pedidos atuais
            return View(produtosAPP.ShowAll());
        }

        public ActionResult PedirProdutos(string id)
        {
            var produtos = produtosAPP.ListarPorId(id);
            if (produtos == null)
                return HttpNotFound();

            return View(produtos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Produtos produto, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fotoNome = Path.GetFileName(file.FileName);
                    var caminho = Path.Combine(Server.MapPath("~/ImgProdutos"), fotoNome);
                    file.SaveAs(caminho);
                    produto.Imagem = fotoNome;
                    produtosAPP.Salvar(produto);
                }
                else
                {
                    produtosAPP.Salvar(produto);
                }

            }
            return RedirectToAction("Listarprodutos");
        }
    }
}