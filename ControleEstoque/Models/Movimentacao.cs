using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ControleEstoque.Models
{
    public class Movimentacao
    {
        public int MovimentacaoId { get; set; }

        //Relacionamento com Produto (chave estrangeira)
        [Required]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        [Required]
        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Required]
        [Display(Name = "Tipo da Movimentação")]
        public string? Tipo { get; set; } // Entrada ou Saída

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Movimentação")]
        public DateTime? DataMovimentacao { get; set; }

        //relacionamento com a tabela de usuarios (identityUser)
        [Display(Name = "Usuário")]
        public string? UsuarioId { get; set; } // O identityUser  usa string como chave primaria
        public IdentityUser? Usuario { get; set; } //Referencia para a classe padrão ao Identity

        [Required]
        [Display(Name = "Observação")]
        [DataType(DataType.MultilineText)]
        public string? Observacao { get; set; }


    }
}
