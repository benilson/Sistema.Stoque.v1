using System.Collections.Generic;
using Sistema.Stoque.v1.Dominio;
using Sistema.Stoque.v1.Dominio.Contrato;

namespace Sistema.Stoque.v1.Aplicacao
{
    public class UsuarioAplicacao
    {
        private readonly IRepositorio<Usuario> repositorio;
        // |-- objecto usado pela interface 
        public UsuarioAplicacao(IRepositorio<Usuario> repo)
        { //Recebe uma classe que sera usada pela Interface
            repositorio = repo;
        }
        //Executa o metodo Insert se o Id for menor que zero caso seja maior executa o metodo Update
        public void Salvar(Usuario usuario)
        {
            repositorio.Salvar(usuario);
        }
        // Elimina o Gerador da BD pelo ID
        public void Delete(Usuario usuario)
        {
            repositorio.Deletar(usuario);
        }
        //Exibe todos os registos da BD
        public IEnumerable<Usuario> ShowAll()
        {
            return repositorio.MostrarTodos();
        }
        //lista os registos da BD por -->ID
        public Usuario ListarPorId(string id)
        {
            return repositorio.ListarPorId(id);
        }
    }
}
