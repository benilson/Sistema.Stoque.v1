using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistema.Stoque.v1.Aplicacao;
using Sistema.Stoque.v1.Dominio;

namespace Sistema.Stoque.v1.UI.Controllers
{

    public class ProdutosController : Controller
    {    //para guardar os valores anteriores que serão adicionados
        public System.Globalization.CultureInfo regiaoData = new System.Globalization.CultureInfo("pt-BR");
        public static int acrescimos;
        public static int decrescimos;
        public static string idProduto;
        public static int idEntrada;
        public static int idSaida;
        public static int quantidadeAtual;
        public static int saidaQuantidadeAtual;
        public readonly ProdutosAplicacao produtosAPP;
        public readonly SaidasAplicacao saidasAPP;
        public readonly EntradaAplicacao entradaAPP;


        public ProdutosController()
        {
            produtosAPP = AplicacaoConstrutores.ProdutosAplicacaoConstrutor();
            saidasAPP = AplicacaoConstrutores.SaidasAplicacaoConstrutor();
            entradaAPP = AplicacaoConstrutores.EntradasAplicacaoConstrutor();
        }

        //===============================================PRODUTOS ESGOTADOS
        public ActionResult ProdutosEsgotados()
        {
            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");
            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidas1.First().DataSaida, culture).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidas1.Last().DataSaida, culture).Year;
            }

            var totalPrecos = 0;
            var produtos = produtosAPP.ShowAll().ToList();
            var produtosEsgotados = new List<Produtos>();
            foreach(var produto in produtos)
            {
                if(produto.Quantidade <= 2)
                {
                    produtosEsgotados.Add(produto);
                }
            }
            ViewBag.TotalPreco = totalPrecos;
            return View(produtosEsgotados);
        }
        //Mostra produtos filtrados ============================================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProdutosEsgotados(string nome, string optSector, string mes, string ano)
        {
            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");
            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidas1.First().DataSaida, culture).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidas1.Last().DataSaida, culture).Year;
            }

            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            ViewBag.Nome = nome;
            ViewBag.OptSector = optSector;

            ViewBag.Mes = mes != "" ? meses[Convert.ToInt16(mes)] : "";
            ViewBag.Ano = ano;

            var resultadoFiltros = produtosAPP.FiltrosNomeDataCategoria(nome, mes, ano, optSector);
            var produtos = produtosAPP.ShowAll().ToList();
            var produtosEsgotados = new List<Produtos>();

            foreach (var produto in resultadoFiltros)
            {
                if (produto.Quantidade <= 2)
                {
                    produtosEsgotados.Add(produto);
                }
            }
            return View(produtosEsgotados);

        }
        //===============================================PRODUTOS ESGOTADOS
        // GET: produto
        public ActionResult CadastrarProdutos()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarProdutos(Produtos produtos, HttpPostedFileBase file, string catregoria)
        {

            //validação de imagem====================================
            var fotoNome = "";
            if (file != null)
            {
                fotoNome = Path.GetFileName(file.FileName);
                if (file.ContentLength < 51000000)
                {
                    if (file.ContentType.Contains("image"))
                    {

                        var caminho = Path.Combine(Server.MapPath("~/ImgProdutos"), fotoNome);
                        file.SaveAs(caminho);
                    }
                    else
                    {
                        ViewBag.ErroImg = "Por favor insira um arquivo do tipo PNG ou JPG";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErroImg = "Por favor Selecione uma imagem menor";
                }
            }//fecha validação imagem

            var user = Session["usuario"] as Usuario;
            produtos.Imagem = fotoNome;
            produtos.NomeUser = user.NomeUsuario;
            produtos.Categoria = catregoria;
            produtos.Quantidade = 0;
            produtosAPP.Salvar(produtos);
            return RedirectToAction("Listarprodutos");
        }

        public ActionResult Listarprodutos()
        {
            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");
            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidas1.First().DataSaida, culture).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidas1.Last().DataSaida, culture).Year;
            }

            var totalPrecos = 0;
            var produtos = produtosAPP.ShowAll();

            ViewBag.TotalPreco = totalPrecos;
            return View(produtos);
        }
        //Mostra produtos filtrados ============================================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarProdutos(string nome, string optSector, string mes, string ano)
        {
            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");
            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidas1.First().DataSaida, culture).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidas1.Last().DataSaida, culture).Year;
            }

            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            ViewBag.Nome = nome;
            ViewBag.OptSector = optSector;

            ViewBag.Mes = mes != "" ? meses[Convert.ToInt16(mes)] : "";
            ViewBag.Ano = ano;

            var resultadoFiltros = produtosAPP.FiltrosNomeDataCategoria(nome, mes, ano, optSector);

            return View(resultadoFiltros);

        }

        //Realiza a Edição dos Produtos
        public ActionResult Editar(string id)
        {
            var produtos = produtosAPP.ListarPorId(id);
            if (produtos == null)
                return HttpNotFound();

            ViewBag.Quantidadeatual = produtos.Quantidade;
            

            return View(produtos);
        }
        //Realiza a Edição dos Produtos
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
        //Elimina os produtos da BD==============================================================
        public ActionResult Eliminar(string id)
        {
            var produtos = produtosAPP.ListarPorId(id);

            if (produtos == null)
                return HttpNotFound();

            return View(produtos);
        }

        //Elimina os Produtos da BD==============================================================
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(string id)
        {
            var produto = produtosAPP.ListarPorId(id);
            produtosAPP.Delete(produto);

            return RedirectToAction("Listarprodutos");
        }

        //Realiza a Aumento das quantidades dos Produtos=========================================
        public ActionResult AumentarProdutos(string id)
        {
            Produtos produto;
            if (id != null)
            {
                produto = produtosAPP.ListarPorId(id);
                if (produto == null)
                    return HttpNotFound();
            } else {
                produto = produtosAPP.ListarPorId(idProduto);
                if (produto == null)
                    return HttpNotFound();
            }

            ViewBag.Quantidadeatual = acrescimos = produto.Quantidade;
  

            produto.Quantidade = 0;
            return View(produto);
        }

        //Realiza o Aumento das quantidades de Produtos=========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AumentarProdutos(Produtos produto, HttpPostedFileBase file)
        {
            var fotoNome = "";
            if (file != null)
            {
                fotoNome = Path.GetFileName(file.FileName);
                if (file.ContentLength < 51000000)
                {
                    if (file.ContentType.Contains("image"))
                    {

                        var caminho = Path.Combine(Server.MapPath("~/ImgProdutos"), fotoNome);
                        file.SaveAs(caminho);
                    }
                    else
                    {
                        ViewBag.ErroImg = "Por favor insira um arquivo do tipo PNG ou JPG";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErroImg = "Por favor Selecione uma imagem menor";
                }
            }

            ViewBag.Quantidadeatual = acrescimos;

            if (ModelState.IsValid)
            {
                
                var user = Session["usuario"] as Usuario;
                var entradas = new Entradas();
                entradas.NomeUser = user.NomeUsuario;
                entradas.Quantidade = produto.Quantidade;
                entradas.Id_produto = produto.Id_produto;
                entradas.NomeProduto = produto.NomeProduto;
                entradas.Imagem = produto.Imagem == null? fotoNome : produto.Imagem;


                if (entradas.Quantidade >= 0)
                {
                    entradaAPP.Salvar(entradas);
                    produto.Quantidade += acrescimos;
                    produtosAPP.Salvar(produto);
                } else {
                    ViewBag.Erro = "O campo Quantidade não pode ser negativo";
                    return View(produto);
                }

            }
            return RedirectToAction("Listarprodutos");
        }

        //Realiza a Diminuição das quantidades dos Produtos=========================================
        public ActionResult DiminuirProdutos(string id)
        {
            var ListaDeSectores = new List<string>()
            {
                "ABP", "Administrativo", "Compras", "Contabilidade", "Contas de Consumo",
                "DTI", "Financeiro", "Juridico", "Locação", "Membros e Obreiros", "Migração",
                "Patrimonio", "Provincias", "RH", "Secretaria", "Contratos", "Direcção",
                "Sector Tecnico"
            };
            ViewBag.Sectores = ListaDeSectores.ToList();

            Produtos produto;
            if (id != null)
            {
                produto = produtosAPP.ListarPorId(id);
                if (produto == null)
                    return HttpNotFound();
            } else {
                produto = produtosAPP.ListarPorId(idProduto);
                if (produto == null)
                    return HttpNotFound();
            }


            ViewBag.Quantidadeatual = decrescimos = produto.Quantidade;


            produto.Quantidade = 0;
            return View(produto);
        }

        //Realiza a Diminuição das quantidades de Produtos=========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DiminuirProdutos(Produtos produto, string optSector)
        {
            var ListaDeSectores = new List<string>()
            {
                "ABP", "Administrativo", "Compras", "Contabilidade", "Contas de Consumo",
                "DTI", "Financeiro", "Juridico", "Locação", "Membros e Obreiros", "Migração",
                "Patrimonio", "Provincias", "RH", "Secretaria", "Contratos", "Direcção",
                "Sector Tecnico"
            };
            ViewBag.Sectores = ListaDeSectores.ToList();

            ViewBag.Quantidadeatual = decrescimos;

            if (ModelState.IsValid)
            {
                var user = Session["usuario"] as Usuario;
                var saida = new Saidas();
                saida.NomeUser = user.NomeUsuario;
                saida.Quantidade = produto.Quantidade;
                saida.Id_produto = produto.Id_produto;
                saida.NomeProduto = produto.NomeProduto;
                saida.Sector = optSector;
                saida.Imagem = produto.Imagem;


                saidasAPP.Salvar(saida);
                if (decrescimos < produto.Quantidade) {
                    ViewBag.erro = "A saida não pode ser maior que a quantidade atual em stock";
                    return View();
                }
                if (produto.Quantidade >= 0)
                {
                    produto.Quantidade = decrescimos - produto.Quantidade;
                    produtosAPP.Salvar(produto);

                } else {
                    ViewBag.erro = "A quantidade ou preço não pode ser um valor negativo";
                }


            }
            return RedirectToAction("Listarprodutos");
        }
        //lista Todas a saidas por produtos=========================================
        public ActionResult ListarprodutosSaidas(string id)
        {
            var totalPrecos = 0;
            var produto = produtosAPP.ListarPorId(id);
            ViewBag.Quantidadeatual = produto.Quantidade;
            ViewBag.Imagem = produto.Imagem;
            ViewBag.idProduto = id;
            idProduto = id;



            var saidas = saidasAPP.ShowAll().ToList();
            var saidaProduto = new List<Saidas>();

            foreach (var saida in saidas) {
                if (saida.Id_produto == Convert.ToInt32(id)) {
                    saidaProduto.Add(saida);
                }
            }
            ViewBag.TotalPreco = totalPrecos;

            if (produto.Quantidade == 0) {
                return RedirectToAction("NaoExiteQuantidades");
            }
            return View(saidaProduto);
        }

        public ActionResult NaoExiteQuantidades() {

            return View();
        }

        //Lista Todas As Entradas Dos Produtos=========================================
        public ActionResult ListarprodutosEntradas(string id)
        {
            var totalPrecos = 0;
            var produto = produtosAPP.ListarPorId(id);
            ViewBag.Quantidadeatual = produto.Quantidade;
            ViewBag.Imagem = produto.Imagem;
            ViewBag.idProduto = id;


            var entradas = entradaAPP.ShowAll().ToList();
            var novasEntradas = new List<Entradas>();
            foreach (var entrada in entradas) {
                if (entrada.Id_produto == Convert.ToInt32(id)) {

                    novasEntradas.Add(entrada);
                }
               
            }
            ViewBag.TotalPreco = totalPrecos;

            if (entradas == null) {
                return RedirectToAction("AumentarProdutos");
            }
            return View(novasEntradas.ToList());
        }

        //===================================Saidas Produtos===================================\\

        //Lista Todas As Saidas Por Mes =============================================================
        public ActionResult SaidasPorMes() {
            var ContadorMensal = 0;
            var quantiProd = new int[12];

            var saidas = saidasAPP.ShowAll().ToList();
            string[] meses = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            var culture = new System.Globalization.CultureInfo("pt-BR");
            for (var i = 0; i < meses.Length; i++) {
                foreach (var saida in saidas) {
                    if (Convert.ToDateTime(saida.DataSaida, culture).Month == (i + 1)) {
                        
                        ContadorMensal += 1;
                    }
                }
                quantiProd[i] = ContadorMensal;
                ContadorMensal = 0;
            }

            ViewBag.meses = meses;
            ViewBag.QuantiProd = quantiProd;

            return View();
        }


        //Pequisa Saidas Por Mes==============================================================
        public ActionResult PesquisarSaidasPorMes(string id) {

            var totalPrecos = 0;
            string[] meses = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };

            var saidas = saidasAPP.ShowAll().ToList();
            var produtos = produtosAPP.ShowAll().ToList();
            var todaSaidasDesteMes = new List<Saidas>();
            ViewBag.Mes = meses[Convert.ToInt32(id)];
            var novoId = Convert.ToInt32(id) + 1;
            
            foreach (var saida in saidas) { 
                if (Convert.ToDateTime(saida.DataSaida, regiaoData).Month == novoId) {
                    todaSaidasDesteMes.Add(saida);
                }
            }

            if(todaSaidasDesteMes.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(todaSaidasDesteMes.First().DataSaida, regiaoData).Year;
                ViewBag.TotalPreco = totalPrecos;
            }

            return View(todaSaidasDesteMes);
        }//fecha metodo PesquisarSaidasPorMes

        //Lista todos os produtos saidos ============================================================================
        public ActionResult ListarProdutosSaidos() {
            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");

            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidasAPP.ShowAll().First().DataSaida, regiaoData).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidasAPP.ShowAll().Last().DataSaida, regiaoData).Year;
            }


            var totalPrecos = 0;

            var saidas = saidasAPP.ShowAll().ToList();


            ViewBag.TotalPreco = totalPrecos;

            return View(saidas.ToList());
        }//fecha metodo ListarProdutosSaidos

        //Mostra produtos filtrados ============================================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarProdutosSaidos(string nome, string optSector, string mes, string ano)
        {
            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");
            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidas1.First().DataSaida, culture).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidas1.Last().DataSaida, culture).Year;
            }

            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            ViewBag.Nome = nome;
            ViewBag.OptSector = optSector;
            ViewBag.Mes = mes != "" ? meses[Convert.ToInt32(mes)] : "";
            ViewBag.Ano = ano;

            var resultadoFiltros = saidasAPP.FiltrosNomeDataCategoria(nome, mes, ano, optSector);

            return View(resultadoFiltros);

        }
        //===================================Entradas Proutos===================================

        //Lista Todas As Saidas Por Mes =============================================================
        public ActionResult EntradasPorMes()
        {
            var ContadorMensal = 0;
            var quantiProd = new int[12];

            var entradas = entradaAPP.ShowAll().ToList();
            string[] meses = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };

            for (var i = 0; i < meses.Length; i++)
            {
                foreach (var entrada in entradas)
                {
                    if (Convert.ToDateTime(entrada.DataEntrada, regiaoData).Month == (i + 1))
                    {
                        ContadorMensal += 1;
                    }
                }
                quantiProd[i] = ContadorMensal;
                ContadorMensal = 0;
            }

            ViewBag.meses = meses;
            ViewBag.QuantiProd = quantiProd;

            return View();
        }
        //Pequisa Saidas Por Mes==============================================================
        public ActionResult PesquisarEntradasPorMes(string id)
        {
            var totalPrecos = 0;
            string[] meses = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            var entradas = entradaAPP.ShowAll().ToList();
            var produtos = produtosAPP.ShowAll().ToList();
            var todaEntradasDesteMes = new List<Entradas>();

            ViewBag.Mes = meses[Convert.ToInt32(id)];
            
            var novoId = Convert.ToInt32(id) + 1;

            foreach (var entrada in entradas) {
                if (Convert.ToDateTime(entrada.DataEntrada, regiaoData).Month == novoId)
                    todaEntradasDesteMes.Add(entrada);
            }
            if(todaEntradasDesteMes.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(todaEntradasDesteMes.First().DataEntrada, regiaoData).Year;
                ViewBag.TotalPreco = totalPrecos;
            }

            return View(todaEntradasDesteMes);
        }//fecha metodo PesquisarEntradasPorMes

        public ActionResult ListarProdutosEntrados()
        {
            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;
            //pega o primeiro e ultimo ano que sera passado para o form
            var entradas1 = entradaAPP.ShowAll().ToList();
            if (entradas1.Count > 0)
            { 
                ViewBag.PrimeiroAno = Convert.ToDateTime(entradaAPP.ShowAll().First().DataEntrada, regiaoData).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(entradaAPP.ShowAll().Last().DataEntrada, regiaoData).Year;
            }


            var totalPrecos = 0;
            var entradas = entradaAPP.ShowAll().ToList();

            ViewBag.TotalPreco = Convert.ToDecimal(totalPrecos);
            return View(entradaAPP.ShowAll().ToList());
        }//fecha metodo ListarProdutosSaidos

        //Lista de Produtos Entrados Pesquisados por mes
        //Mostra produtos filtrados ============================================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarProdutosEntrados(string nome, string optSector, string mes, string ano)
        {
            //pega o primeiro e ultimo ano que sera passado para o form
            var culture = new System.Globalization.CultureInfo("pt-BR");
            var saidas1 = saidasAPP.ShowAll().ToList();
            if (saidas1.Count > 0)
            {
                ViewBag.PrimeiroAno = Convert.ToDateTime(saidas1.First().DataSaida, culture).Year;
                ViewBag.ultimoAno = Convert.ToDateTime(saidas1.Last().DataSaida, culture).Year;
            }

            string[] meses = {"", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                               "Julho", "Agosto", "Setembro","Outubro","Novembro","Dezembro" };
            TempData["Meses"] = meses;

            string[] categorias = { "Higiene", "Escritorio", "Consumo", "Suprimentos" };
            TempData["Categoria"] = categorias;

            ViewBag.Nome = nome;
            ViewBag.OptSector = optSector;
            ViewBag.Mes = mes != "" ? meses[Convert.ToInt32(mes)] : "";
            ViewBag.Ano = ano;

            var resultadoFiltros = entradaAPP.FiltrosNomeDataCategoria(nome, mes, ano, optSector);

            return View(resultadoFiltros);

        }


        //Realiza A Alteração das entradas do produto em caso de erros=========================================
        public ActionResult EditarProdutoEntrada(string id)
        {
            Produtos produto = null;
            if (id != null)
            {
                var entrada = entradaAPP.ListarPorId(id);
                acrescimos = entrada.Quantidade;//pega quantidade da entrada que sera atualizada
                idEntrada = entrada.Id_EntradaProd;
                produto = produtosAPP.ListarPorId(entrada.Id_produto.ToString());
                ViewBag.Quantidadeatual = produto.Quantidade;
            
                quantidadeAtual = produto.Quantidade;//adiciona a quantidade para ser subtraida pelo quantidade do aumento

                produto.Quantidade = entrada.Quantidade;

                if (produto == null)
                    return HttpNotFound();
            }
            else
            {
                var entrada = entradaAPP.ListarPorId(id);
                acrescimos = entrada.Quantidade;//pega quantidade da entrada que sera atualizada
                idEntrada = Convert.ToInt16(id);
                produto = produtosAPP.ListarPorId(entrada.Id_produto.ToString());
                ViewBag.Quantidadeatual = produto.Quantidade;

                quantidadeAtual = produto.Quantidade;//adiciona a quantidade para ser subtraida pelo quantidade do aumento

                produto.Quantidade = entrada.Quantidade;

                if (produto == null)
                    return HttpNotFound();
            }

           
            return View(produto);
        }

        //Realiza A Alteração das entradas do produto em caso de erros=========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProdutoEntrada(Produtos produto, HttpPostedFileBase file)
        {

            var fotoNome = "";
                if (file != null)
                {
                    fotoNome = Path.GetFileName(file.FileName);
                    if (file.ContentLength < 51000000)
                    {
                        if (file.ContentType.Contains("image"))
                        {

                            var caminho = Path.Combine(Server.MapPath("~/ImgProdutos"), fotoNome);
                            file.SaveAs(caminho);
                        }
                        else
                        {
                            ViewBag.ErroImg = "Por favor insira um arquivo do tipo PNG ou JPG";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.ErroImg = "Por favor Selecione uma imagem menor";
                    }
                }
            

            ViewBag.Quantidadeatual = produto.Quantidade;
            if (produto.Quantidade >= 0)
            {
                if (ModelState.IsValid)
                {

                    var user = Session["usuario"] as Usuario;
                    var entrada = entradaAPP.ListarPorId(idEntrada.ToString());
                    entrada.Quantidade = produto.Quantidade;
                    produto.Imagem = fotoNome != "" ? fotoNome : "";

                    entradaAPP.Salvar(entrada);
                    produto.Quantidade = (quantidadeAtual - acrescimos) + produto.Quantidade;

                    produtosAPP.Salvar(produto);
                }
                return RedirectToAction("Listarprodutos");
            } else {
                ViewBag.Erro = "O preço e a quantidade de ser positiva";
                return View(produto);
            }
            
        }

        //Realiza A Alteração das entradas do produto em caso de erros=========================================
        public ActionResult EditarProdutoSaida(string id)
        {
            Produtos produto = null;
            if (id != null)
            {
                var saida = saidasAPP.ListarPorId(id);
                acrescimos = saida.Quantidade;//pega quantidade da entrada que sera atualizada
                idSaida = saida.Id_saidasProd;
                produto = produtosAPP.ListarPorId(saida.Id_produto.ToString());
                ViewBag.Quantidadeatual = saidaQuantidadeAtual = produto.Quantidade;

                quantidadeAtual = produto.Quantidade;//adiciona a quantidade para ser subtraida pelo quantidade do aumento

                produto.Quantidade = saida.Quantidade;

                if (produto == null)
                    return HttpNotFound();
            }

            return View(produto);
        }

        //Realiza A Alteração das entradas do produto em caso de erros=========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProdutoSaida(Produtos produto)
        {

            ViewBag.Quantidadeatual = saidaQuantidadeAtual;
            if (produto.Quantidade >= 0)
            {
                if (ModelState.IsValid)
                {

                    var user = Session["usuario"] as Usuario;
                    var saida = saidasAPP.ListarPorId(idSaida.ToString()); ;
                    saida.Quantidade = produto.Quantidade;

                    saidasAPP.Salvar(saida);
                    produto.Quantidade = (quantidadeAtual + acrescimos) - produto.Quantidade;

                    produtosAPP.Salvar(produto);
                }
                return RedirectToAction("Listarprodutos");
            } else {
                ViewBag.Erro = "O preço ou quantidade não deve ser um valor negativo";
                return View(produto);
            }
        }
    }
}