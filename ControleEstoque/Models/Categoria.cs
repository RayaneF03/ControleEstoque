using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        //DataAnnotations: Validações nos campos do formulário, exibe mensagens de erro;
        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres")]
        [Display(Name = "Nome da Categoria")]
        public string? Nome { get; set; }
    }
}
