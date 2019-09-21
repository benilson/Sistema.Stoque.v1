using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;
using System;
using System.Collections.Generic;

namespace Sistema.Stoque.v1.Repositorio
{
    public class SaidasRepositorioADO:IRepositorio<Saidas>
    {
        private Contexto con;
        //Insere o registo na TB
        private void Insert(Saidas saidas)
        {
            var strQuery = "INSERT INTO tb_SaidasProdu(DataSaida, Quantidade, HoraSaida, NomeUser, NomeProduto, Imagem, Id_produto, Categoria, Sector) ";
            strQuery +=
                $" VALUES('{DateTime.Now.ToShortDateString()}', {saidas.Quantidade}, '{DateTime.Now.ToShortTimeString()}', '{saidas.NomeUser}','{saidas.NomeProduto}', '{saidas.Imagem}', '{saidas.Id_produto}', '{saidas.Categoria}', '{saidas.Sector}')";

            using(con = new Contexto())
            {
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Atualiza o registo na TB
        private void Update(Saidas saidas)
        {
            var strQuery = "UPDATE tb_SaidasProdu SET ";
            strQuery += $"DataSaida = '{saidas.DataSaida}', ";
            strQuery += $"Quantidade = '{saidas.Quantidade}', ";
            strQuery += $"NomeProduto = '{saidas.NomeProduto}', ";
            strQuery += $"Imagem = '{saidas.Imagem}', ";
            strQuery += $"Id_produto = '{saidas.Id_produto}' ";
            strQuery += $"WHERE Id_SaidaProd = '{saidas.Id_saidasProd}' ";

            using(con = new Contexto())
            {
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Saidas saidas)
        {

            if(saidas.Id_saidasProd > 0)
                Update(saidas);
            else
                Insert(saidas);
        }
        // Elimina o Gerador da BD pelo ID
        public void Deletar(Saidas saidas)
        {
            using(con = new Contexto())
            {
                var strQuery = $"DELETE tb_SaidasProdu WHERE Id_SaidaProd = '{saidas.Id_saidasProd}' ";
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Exibe todos os registos da BD
        public IEnumerable<Saidas> MostrarTodos()
        {
            var listaSaidas = new List<Saidas>();
            var strQuery = "SELECT * FROM tb_SaidasProdu";
            using(con = new Contexto())
            {
                var read = con.ExecQueryComRetorno(strQuery);
                while(read.Read())
                {
                    var saidas = new Saidas();
                    saidas.Id_saidasProd = Convert.ToInt16(read["Id_SaidaProd"]);
                    saidas.Quantidade = Convert.ToInt32(read["Quantidade"]);
                    saidas.NomeProduto = read["NomeProduto"].ToString();
                    saidas.Imagem = read["Imagem"].ToString();
                    saidas.Id_produto = Convert.ToInt32(read["Id_produto"]);
                    saidas.NomeUser = read["NomeUser"].ToString();
                    saidas.HoraSaida = read["HoraSaida"].ToString();
                    saidas.DataSaida = read["DataSaida"].ToString();
                    saidas.Categoria = read["Categoria"].ToString();

                    listaSaidas.Add(saidas);
                }
            }
            return listaSaidas;
        }
        //lista os registos da BD por -->ID
        public Saidas ListarPorId(string id)
        {
            Saidas saidas = null;
            using(con = new Contexto())
            {
                var strQuery = $"SELECT * FROM tb_SaidasProdu WHERE Id_SaidaProd = {id}";
                var read = con.ExecQueryComRetorno(strQuery);
                if(read.Read())
                {
                    saidas = new Saidas();
                    saidas.Id_saidasProd = Convert.ToInt16(read["Id_SaidaProd"]);
                    saidas.Quantidade = Convert.ToInt32(read["Quantidade"]);
                    saidas.NomeProduto = read["NomeProduto"].ToString();
                    saidas.Imagem = read["Imagem"].ToString();
                    saidas.Id_produto = Convert.ToInt32(read["Id_produto"]);
                    saidas.NomeUser = read["NomeUser"].ToString();
                    saidas.HoraSaida = read["HoraSaida"].ToString();
                    saidas.DataSaida = read["DataSaida"].ToString();
                    saidas.Categoria = read["Categoria"].ToString();

                    return saidas;
                }
                return saidas;
            }
        }
    }
}
