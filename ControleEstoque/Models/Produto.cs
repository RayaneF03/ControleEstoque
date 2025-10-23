using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        //DataAnnotations: Validações nos campos do formulário, exibe mensagens de erro;
        [Required(ErrorMessage = "O nome do Produto é obrigatório")]
        [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres")]
        [Display(Name = "Nome do Produto")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O nome da Marca é obrigatório")]
        [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres")]
        [Display(Name = "Nome da Marca")]
        public string? Marca { get; set; }

        //Relacionamento com Categoria (chave estrangeira)
        [Required(ErrorMessage = "A Categoria é Obrigatória")]
        [Display(Name = "Nome da Categoria")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "O Estoque atual não pode ser negativo")]
        [Display(Name = "Estoque Atual")]
        public int EstoqueAtual { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "O Estoque minimo não pode ser negativo")]
        [Display(Name = "Estoque Minimo")]
        public int EstoqueMinimo { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Informe uma data valida")]
        [Display(Name = "Data de Validade")]
        public DateTime? DataValidade { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Preço de Custo")]
        public decimal? PrecoCusto { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Preço de Venda")]
        public decimal? PrecoVenda { get; set; }

        [Display(Name = "Descrição Detalhada")]
        [DataType(DataType.MultilineText)]
        public string? Descricao { get; set; }
    }
}
