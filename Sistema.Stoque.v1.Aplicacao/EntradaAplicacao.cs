using System;
using System.Collections.Generic;
using System.Linq;
using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;

namespace Sistema.Stoque.v1.Aplicacao
{
   public class EntradaAplicacao
    {
        private readonly IRepositorio<Entradas> repositorio;
        // |-- objecto usado pela interface 
        public EntradaAplicacao(IRepositorio<Entradas> repo)
        { //Recebe uma classe que sera usada pela Interface
            repositorio = repo;
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Entradas entrada)
        {
            repositorio.Salvar(entrada);
        }
        // Elimina o Gerador da BD pelo ID
        public void Delete(Entradas entrada)
        {
            repositorio.Deletar(entrada);
        }
        //Exibe todos os registos da BD
        public IEnumerable<Entradas> ShowAll()
        {
            return repositorio.MostrarTodos();
        }
        //lista os registos da BD por -->ID
        public Entradas ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }

        //retorna os produtos de acordo com os parametros dos filtros
        public List<Entradas> FiltrosNomeDataCategoria(string nome, string mes, string ano, string categoria)
        {
            if (mes == "" && ano == "" && nome == "")//por categoria
            {
                return repositorio.MostrarTodos().Where(p => p.Categoria == categoria).ToList();
            }
            if (nome == "" && ano == "")//todos os meses
            {
                return repositorio.MostrarTodos().Where(p => Convert.ToDateTime(p.DataEntrada).Month == Convert.ToInt32(mes)).ToList();
            }
            if (mes == "" && ano == "")//nome
            {
                return repositorio.MostrarTodos().Where(p => p.NomeProduto.ToUpper().Contains(nome.ToUpper())).ToList();

            }
            if (mes == "" && nome == "" && categoria == "")
            {
                return repositorio.MostrarTodos().Where(p => Convert.ToDateTime(p.DataEntrada).Year == Convert.ToInt32(ano)).ToList();
            }
            if (ano == "")
            {

                return repositorio.MostrarTodos().Where(p => p.NomeProduto.ToUpper().Contains(nome.ToUpper()) && Convert.ToDateTime(p.DataEntrada).Month == Convert.ToInt32(mes)).ToList();
            }
            else {
                return repositorio.MostrarTodos().Where(p => p.NomeProduto.ToUpper().Contains(nome.ToUpper()) &&
                                                                       Convert.ToDateTime(p.DataEntrada).Year == Convert.ToInt32(ano) &&
                                                                       Convert.ToDateTime(p.DataEntrada).Month == Convert.ToInt32(mes)).ToList();
            }

            //p.Categoria == categoria  incluir


        }
    }
}
