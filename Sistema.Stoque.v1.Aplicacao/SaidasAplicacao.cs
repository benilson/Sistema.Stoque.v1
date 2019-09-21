using System;
using System.Collections.Generic;
using System.Linq;
using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;

namespace Sistema.Stoque.v1.Aplicacao
{
   public class SaidasAplicacao
    {
        private readonly IRepositorio<Saidas> repositorio;
        // |-- objecto usado pela interface 
        public SaidasAplicacao(IRepositorio<Saidas> repo)
        { //Recebe uma classe que sera usada pela Interface
            repositorio = repo;
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Saidas saidas)
        {
            repositorio.Salvar(saidas);
        }
        // Elimina o Gerador da BD pelo ID
        public void Delete(Saidas saidas)
        {
            repositorio.Deletar(saidas);
        }
        //Exibe todos os registos da BD
        public IEnumerable<Saidas> ShowAll()
        {
            return repositorio.MostrarTodos();
        }
        //lista os registos da BD por -->ID
        public Saidas ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }
        //retorna os produtos de acordo com os parametros dos filtros
        public List<Saidas> FiltrosNomeDataCategoria(string nome, string mes, string ano, string categoria)
        {
            if (mes == "" && ano == "" && nome == "")//por categoria
            {
                return repositorio.MostrarTodos().Where(p => p.Categoria == categoria).ToList();
            }
            if (nome == "" && ano == "")//todos os meses
            {
                return repositorio.MostrarTodos().Where(p => Convert.ToDateTime(p.DataSaida).Month == Convert.ToInt32(mes)).ToList();
            }
            if (mes == "" && ano == "")//nome
            {
                return repositorio.MostrarTodos().Where(p => p.NomeProduto.ToUpper().Contains(nome.ToUpper())).ToList();

            }
            if (mes == "" && nome == "" && categoria == "")
            {
                return repositorio.MostrarTodos().Where(p => Convert.ToDateTime(p.DataSaida).Year == Convert.ToInt32(ano)).ToList();
            }
            if (ano == "")
            {

                return repositorio.MostrarTodos().Where(p => p.NomeProduto.ToUpper().Contains(nome.ToUpper()) && Convert.ToDateTime(p.DataSaida).Month == Convert.ToInt32(mes)).ToList();
            }
            else {
                return repositorio.MostrarTodos().Where(p => p.NomeProduto.ToUpper().Contains(nome.ToUpper()) &&
                                                                       Convert.ToDateTime(p.DataSaida).Year == Convert.ToInt32(ano) &&
                                                                       Convert.ToDateTime(p.DataSaida).Month == Convert.ToInt32(mes)).ToList();
            }

            //p.Categoria == categoria  incluir


        }
    }
}
