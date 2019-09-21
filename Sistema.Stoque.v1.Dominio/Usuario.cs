using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Stoque.v1.Dominio
{  
        public class Usuario
        {
            [DisplayName("ID")]
            public int Id_Usuario { get; set; }

            [DisplayName("Nome")]
            public string NomeUsuario { get; set; }

            [DisplayName("Nome Completo")]
            public string NomeCompleto { get; set; }

            [DisplayName("Perfil")]
            public string PerfilUsuario { get; set; }

            [DisplayName("Telf")]
            public string telfUsuario { get; set; }

            [DisplayName("Foto")]
            public string urlFoto { get; set; }

            [DisplayName("Senha")]
            [Compare("senhaRepetida", ErrorMessage = "As senhas não são iguais")]
            public string SenhaUsuario { get; set; }

            [DisplayName("Repetir Senha")]
            public string senhaRepetida { get; set; }
    }
}
