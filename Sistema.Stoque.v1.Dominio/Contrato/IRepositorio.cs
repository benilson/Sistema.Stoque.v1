using System.Collections.Generic;

namespace Sistema.Stoque.v1.Dominio.Contrato
{
    //interface responsavel pela implementação do CRUD nas classes da app onde T Representa a classe que sera usada no CRUD
    public interface IRepositorio<T> where T : class {

        void Salvar(T entidade);

        void Deletar(T entidade);

        IEnumerable<T> MostrarTodos();

        T ListarPorId(string Id);
    }
}
