namespace Sistema.Stoque.v1.Dominio
{
    public class Entradas
    {
        public int Id_EntradaProd { get; set; }

        public int Quantidade { get; set; }

        public string DataEntrada { get; set; }

        public int Id_produto { get; set; }

        public string NomeProduto { get; set; }

        public string HoraEntrada { get; set; }
        public string Imagem { get; set; }

        public string NomeUser { get; set; }

        public string Categoria { get; set; }


    }
}