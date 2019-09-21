using System;
using System.Data;
using System.Data.SqlClient;

namespace Sistema.Stoque.v1.Repositorio
{
    public class Contexto:IDisposable
    {
        //cria variavel de conexão
        private readonly SqlConnection con;
        //cria e abre conexão
        public Contexto()
        {
            var strCon = @"Data Source=WEB-ANGOLA01\SA;Initial Catalog=Gest;User ID=SA;Password=gest2016;Max Pool Size=100";
            //string strCon = @"Data Source=INT-ANGOLA01\SQLEXPRESS,1433;Initial Catalog=Controle_de_geradores;Persist Security Info=True;User ID=sa;Password=123456789";

            con = new SqlConnection(strCon);
            con.Open();
        }
        //executa query que não possuem retorno ex insert update delete
        public void ExecQuerySemRetorno(string strQuery)
        {
            var cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }
        //executa query que possuem retorno ex select 
        public SqlDataReader ExecQueryComRetorno(string strQuery)
        {
            var cmd = new SqlCommand(strQuery, con);
            return cmd.ExecuteReader();
        }
        //libera memoria da maquina
        public void Dispose() { }

    }
}
