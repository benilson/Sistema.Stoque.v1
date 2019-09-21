using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;
using System;
using System.Collections.Generic;

namespace Sistema.Stoque.v1.Repositorio
{
    public class UsuarioRepositorioADO:IRepositorio<Usuario>
    {
        private Contexto con;
        //Insere o registo na TB
        private void Insert(Usuario usuario)
        {
            var strQuery = "INSERT INTO tb_Usuarios( NomeCompleto, NomeUsuario, PerfilUsuario, SenhaUsuario, telfUsuario, urlFoto) ";
            strQuery +=
                $" VALUES('{usuario.NomeCompleto}', '{usuario.NomeUsuario}', '{usuario.PerfilUsuario}','{usuario.SenhaUsuario}', '{usuario.telfUsuario}', '{usuario.urlFoto}')";

            using(con = new Contexto())
            {
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Atualiza o registo na TB
        private void Update(Usuario usuario)
        {
            var strQuery = "UPDATE tb_Usuarios SET ";
            strQuery += $"NomeCompleto = '{usuario.NomeCompleto}', ";
            strQuery += $"NomeUsuario = '{usuario.NomeUsuario}', ";
            strQuery += $"PerfilUsuario = '{usuario.PerfilUsuario}', ";
            strQuery += $"SenhaUsuario = '{usuario.SenhaUsuario}', ";
            strQuery += $"telfUsuario = '{usuario.telfUsuario}', ";
            strQuery += $"urlFoto = '{usuario.urlFoto}'";
            strQuery += $"WHERE Id_usuarios = {usuario.Id_Usuario} ";

            using(con = new Contexto())
            {
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Usuario usuario)
        {

            if(usuario.Id_Usuario > 0)
                Update(usuario);
            else
                Insert(usuario);
        }
        // Elimina o Usuario da BD pelo ID
        public void Deletar(Usuario usuario)
        {
            using(con = new Contexto())
            {
                var strQuery = $"DELETE tb_Usuarios WHERE Id_Usuarios = '{usuario.Id_Usuario}' ";
                con.ExecQuerySemRetorno(strQuery);
            }
        }
        //Exibe todos os registos da BD
        public IEnumerable<Usuario> MostrarTodos()
        {
            var listaUsuarios = new List<Usuario>();
            var strQuery = "SELECT * FROM tb_Usuarios";
            using(con = new Contexto())
            {
                var read = con.ExecQueryComRetorno(strQuery);
                while(read.Read())
                {
                    var usuario = new Usuario();
                    usuario.Id_Usuario = Convert.ToInt32(read["Id_Usuarios"]);
                    usuario.NomeCompleto = read["NomeCompleto"].ToString();
                    usuario.NomeUsuario = read["NomeUsuario"].ToString();
                    usuario.PerfilUsuario = read["PerfilUsuario"].ToString();
                    usuario.SenhaUsuario = read["SenhaUsuario"].ToString();
                    usuario.telfUsuario = read["telfUsuario"].ToString();
                    usuario.NomeCompleto = read["NomeCompleto"].ToString();
                    usuario.urlFoto = read["urlFoto"].ToString();
                    listaUsuarios.Add(usuario);
                }
            }
            return listaUsuarios;
        }
        //lista os registos da BD por -->ID
        public Usuario ListarPorId(string id)
        {
            Usuario usuario = null;
            using(con = new Contexto())
            {
                var strQuery = $"SELECT * FROM tb_Usuarios WHERE Id_Usuarios = {id}";
                var read = con.ExecQueryComRetorno(strQuery);
                if(read.Read())
                {
                    usuario = new Usuario();
                    usuario.Id_Usuario = Convert.ToInt32(read["Id_Usuarios"]);
                    usuario.NomeCompleto = read["NomeCompleto"].ToString();
                    usuario.NomeUsuario = read["NomeUsuario"].ToString();
                    usuario.PerfilUsuario = read["PerfilUsuario"].ToString();
                    usuario.telfUsuario = read["telfUsuario"].ToString();
                    usuario.urlFoto = read["urlFoto"].ToString();
                    usuario.SenhaUsuario = read["SenhaUsuario"].ToString();
                    return usuario;
                }
                return usuario;
            }
        }
    }
}
