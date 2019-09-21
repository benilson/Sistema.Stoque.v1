using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;
using System;
using System.Collections.Generic;

namespace Sistema.Stoque.v1.Repositorio
{
    public class ProdutosRepositorioADO:IRepositorio<Produtos>
    {
        private Contexto _con;
        //Insere o registo na TB
        private void Insert(Produtos produto)
        {
            var strQuery = "INSERT INTO tb_Productos(NomeProduto, Quantidade, Imagem,  DataCad, Categoria) ";
            strQuery +=
                $" VALUES('{produto.NomeProduto}', {produto.Quantidade}, '{produto.Imagem}', '{produto.DataCad}', '{produto.Categoria}')";

            using(_con = new Contexto())
            {
                _con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Atualiza o registo na TB
        private void Update(Produtos produto)
        {
            var strQuery = "UPDATE tb_Productos SET ";
            strQuery += $"NomeProduto = '{produto.NomeProduto}', ";
            strQuery += $"Quantidade = {produto.Quantidade}, ";
            strQuery += $"Imagem = '{produto.Imagem}', ";
            strQuery += $"DataCad = '{produto.DataCad}' ";
            strQuery += $"WHERE Id_produto = '{produto.Id_produto}' ";

            using(_con = new Contexto())
            {
                _con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Produtos produto)
        {

            if(produto.Id_produto > 0)
                Update(produto);
            else
                Insert(produto);
        }
        // Elimina o Gerador da BD pelo ID
        public void Deletar(Produtos produto)
        {
            using(_con = new Contexto())
            {
                var strQuery = $"DELETE tb_Productos WHERE Id_produto = '{produto.Id_produto}' ";
                _con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Exibe todos os registos da BD
        public IEnumerable<Produtos> MostrarTodos()
        {
            var listaProdutos = new List<Produtos>();
            var strQuery = "SELECT * FROM tb_Productos";
            using(_con = new Contexto())
            {
                var read = _con.ExecQueryComRetorno(strQuery);
                while(read.Read())
                {
                    var produto = new Produtos();
                    produto.Id_produto = Convert.ToInt16(read["Id_produto"]);
                    produto.NomeProduto = read["NomeProduto"].ToString();
                    produto.Quantidade = Convert.ToInt32(read["Quantidade"]);
                    produto.Imagem = read["Imagem"].ToString();
                    produto.DataCad = read["DataCad"].ToString();
                    produto.Categoria = read["Categoria"].ToString();

                    listaProdutos.Add(produto);
                }
            }
            return listaProdutos;
        }
        //lista os registos da BD por -->ID
        public Produtos ListarPorId(string id)
        {
            using(_con = new Contexto())
            {
                var strQuery = $"SELECT * FROM tb_Productos WHERE Id_produto = {id}";
                var read = _con.ExecQueryComRetorno(strQuery);
                if(!read.Read()) return null;
                var produto = new Produtos
                {
                    Id_produto = Convert.ToInt16(read["Id_produto"]),
                    NomeProduto = read["NomeProduto"].ToString(),
                    Quantidade = Convert.ToInt32(read["Quantidade"]),
                    Imagem = read["Imagem"].ToString(),
                    DataCad = read["DataCad"].ToString(),
                    Categoria = read["Categoria"].ToString()
                };

                return produto;
            }
        }
    }
}
