using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Stoque.v1.Dominio
{
    public class Produtos
    {
        [DisplayName("ID")]
        public int Id_produto { get; set; }

        [Required(ErrorMessage = "O Campo Nome Produto deve ser Preenchido")]
        [DisplayName("Nome do Produto")]
        public string NomeProduto { get; set; }

        [Required(ErrorMessage = "O Campo Quantidade deve ser Preenchido")]
        [DisplayName("Quantidade")]
        public int Quantidade { get; set; }

        [DisplayName("Imagem")]
        public string Imagem { get; set; }
       
        public string DataCad = DateTime.Now.ToShortDateString();

        public string NomeUser { get; set; }

        public string HoraCad { get; set; }

        public List<Saidas> Saidas { get; set; }

        public List<Entradas> Entradas { set; get; }

        public string NomeCompleto { get; set; }

        public string Categoria { get; set; }
    }

}
