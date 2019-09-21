using System;

namespace Sistema.Stoque.v1.Dominio
{
    public class Saidas
    {
        public int Id_saidasProd { get; set; }

        public int Quantidade { get; set; }

        public string DataSaida = DateTime.Now.ToShortDateString();

        public int Id_produto { get; set; }

        public string NomeProduto { get; set; }

        public string Imagem { get; set; }

        public string HoraSaida { get; set; }

        public string NomeUser { get; set; }

        public string Categoria { get; set; }

        public string Sector { get; set; }
    }
}