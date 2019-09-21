using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;
using System;
using System.Collections.Generic;

namespace Sistema.Stoque.v1.Repositorio
{
    public class EntradasRepositorioADO:IRepositorio<Entradas>
    {
        private Contexto con;
        //Insere o registo na TB
        private void Insert(Entradas entradas)
        {
            var strQuery = "INSERT INTO tb_EntradaProdu(DataEntrada, Quantidade, HoraEntrada, NomeUser, NomeProduto, Imagem, Id_produto, Categoria) ";
            strQuery +=
                $" VALUES('{DateTime.Now.ToShortDateString()}', {entradas.Quantidade}, '{DateTime.Now.TimeOfDay}', '{entradas.NomeUser}', '{entradas.NomeProduto}', '{entradas.Imagem}', {entradas.Id_produto}, '{entradas.Categoria}')";

            using(con = new Contexto())
            {
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Atualiza o registo na TB
        private void Update(Entradas entradas)
        {
            var strQuery = "UPDATE tb_EntradaProdu SET ";
            strQuery += $"DataEntrada = '{entradas.DataEntrada}', ";
            strQuery += $"Quantidade = {entradas.Quantidade}, ";
            strQuery += $"Id_produto = '{entradas.Id_produto}', ";
            strQuery += $"Imagem = '{entradas.Imagem}' ";
            strQuery += $"WHERE Id_EntradaProd = '{entradas.Id_EntradaProd}' ";

            using(con = new Contexto())
            {
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Entradas entradas)
        {

            if(entradas.Id_EntradaProd > 0)
                Update(entradas);
            else
                Insert(entradas);
        }
        // Elimina o Gerador da BD pelo ID
        public void Deletar(Entradas Entradas)
        {
            using(con = new Contexto())
            {
                var strQuery = $"DELETE tb_EntradaProdu WHERE Id_EntradaProd = '{Entradas.Id_EntradaProd}' ";
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Exibe todos os registos da BD
        public IEnumerable<Entradas> MostrarTodos()
        {
            var listaEntradas = new List<Entradas>();
            var strQuery = "SELECT * FROM tb_EntradaProdu";
            using(con = new Contexto())
            {
                var read = con.ExecQueryComRetorno(strQuery);
                while(read.Read())
                {
                    var entradas = new Entradas();
                    entradas.Id_EntradaProd = Convert.ToInt16(read["Id_EntradaProd"]);
                    entradas.Quantidade = Convert.ToInt32(read["Quantidade"]);
                    entradas.Id_produto = Convert.ToInt32(read["Id_produto"]);
                    entradas.NomeProduto = read["NomeProduto"].ToString();
                    entradas.Imagem = read["Imagem"].ToString();
                    entradas.DataEntrada = Convert.ToDateTime(read["DataEntrada"]).ToShortDateString();
                    entradas.HoraEntrada = Convert.ToDateTime(read["HoraEntrada"]).ToShortTimeString();
                    entradas.NomeUser = read["NomeUser"].ToString();
                    listaEntradas.Add(entradas);
                }
            }
            return listaEntradas;
        }
        //lista os registos da BD por -->ID
        public Entradas ListarPorId(string id)
        {
            Entradas entradas = null;
            using(con = new Contexto())
            {
                var strQuery = $"SELECT * FROM tb_EntradaProdu WHERE Id_EntradaProd = {id}";
                var read = con.ExecQueryComRetorno(strQuery);
                if(read.Read())
                {
                    entradas = new Entradas();

                    entradas.Id_EntradaProd = Convert.ToInt16(read["Id_EntradaProd"]);
                    entradas.Quantidade = Convert.ToInt32(read["Quantidade"]);
                    entradas.Id_produto = Convert.ToInt32(read["Id_produto"]);
                    entradas.NomeProduto = read["NomeProduto"].ToString();
                    entradas.Imagem = read["Imagem"].ToString();
                    entradas.DataEntrada = Convert.ToDateTime(read["DataEntrada"]).ToShortDateString();
                    entradas.HoraEntrada = Convert.ToDateTime(read["HoraEntrada"]).ToShortTimeString();
                    entradas.NomeUser = read["NomeUser"].ToString();
                    return entradas;
                }
                return entradas;
            }
        }
    }
}
